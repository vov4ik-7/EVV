using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using WebAdvertisements.AdvertApi.DTO;
using WebAdvertisements.AdvertApi.Models;

namespace WebAdvertisements.AdvertApi.Services
{
    public class DynamoDbAdvertStorageService : IAdvertisementStorageService
    {
        private readonly IMapper _mapper;
        private readonly IAmazonDynamoDB _client;

        public DynamoDbAdvertStorageService(IMapper mapper, IAmazonDynamoDB client)
        {
            _mapper = mapper;
            _client = client;
        }

        public async Task<string> Add(Advertisement advertisement)
        {
            var dbModel = _mapper.Map<AdvertisementDb>(advertisement);

            dbModel.Id = Guid.NewGuid().ToString();
            dbModel.CreationTime = DateTime.UtcNow;
            dbModel.Status = AdvertisementStatus.Pending;

            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(dbModel);
            }

            return dbModel.Id;
        }

        public async Task Confirm(ConfirmAdvertisement confirm)
        {
            using var context = new DynamoDBContext(_client);
            var dbModel = await context.LoadAsync<AdvertisementDb>(confirm.Id);
            if (dbModel is null)
                throw new KeyNotFoundException($"A record with ID={confirm.Id} has not been found");
            if (confirm.Status == AdvertisementStatus.Active)
            {
                dbModel.FilePath = confirm.FilePath;
                dbModel.Status = AdvertisementStatus.Active;
                await context.SaveAsync(dbModel);
            }
            else
            {
                await context.DeleteAsync(dbModel);
            }
        }

        public async Task<Advertisement> GetByIdAsync(string id)
        {
            using (var context = new DynamoDBContext(_client))
            {
                var dbModel = await context.LoadAsync<AdvertisementDb>(id);
                if (dbModel != null) return _mapper.Map<Advertisement>(dbModel);
            }
            throw new KeyNotFoundException();
        }

        public async Task<List<Advertisement>> GetAllAsync()
        {
            using (var context = new DynamoDBContext(_client))
            {
                var scanResult =
                    await context.ScanAsync<AdvertisementDb>(new List<ScanCondition>()).GetNextSetAsync();
                return scanResult.Select(item => _mapper.Map<Advertisement>(item)).ToList();
            }
        }
    }
}
