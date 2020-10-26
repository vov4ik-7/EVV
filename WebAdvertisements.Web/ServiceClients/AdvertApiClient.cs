using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebAdvertisements.Web.DTO;

namespace WebAdvertisements.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly string _baseAddress;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public AdvertApiClient(HttpClient client, IMapper mapper, IConfiguration configuration)
        {
            _client = client;
            _mapper = mapper;
            _baseAddress = configuration.GetSection("AdvertApi")["url"];
        }

        public async Task<bool> ConfirmAsync(ConfirmAdvertRequest model)
        {
            var jsonModel = JsonConvert.SerializeObject(model);
            var response = await _client
                .PutAsync(new Uri($"{_baseAddress}/api/v1/advert/confirm"),
                    new StringContent(jsonModel, Encoding.UTF8, "application/json"));
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<CreateAdvertResponse> CreateAsync(CreateAdvertisement model)
        {
            var jsonModel = JsonConvert.SerializeObject(model);
            var response = await _client.PostAsync(new Uri($"{_baseAddress}/api/v1/advert/create"),
                new StringContent(jsonModel, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var createAdvertResponse = JsonConvert
                .DeserializeObject<CreateAdvertResponse>(await response.Content.ReadAsStringAsync());
            return createAdvertResponse;
        }

        public async Task<List<Advertisement>> GetAllAsync()
        {
            var apiCallResponse = await _client
                .GetAsync(new Uri($"{_baseAddress}/api/v1/advert/all"));
            var allAdvertModels = JsonConvert
                .DeserializeObject<List<Advertisement>>(await apiCallResponse.Content.ReadAsStringAsync());
            return allAdvertModels;
        }

        public async Task<Advertisement> GetAsync(string advertId)
        {
            var apiCallResponse = await _client
                .GetAsync(new Uri($"{_baseAddress}/api/v1/advert/{advertId}"));
            var fullAdvert = JsonConvert
                .DeserializeObject<Advertisement>(await apiCallResponse.Content.ReadAsStringAsync());
            return fullAdvert;
        }
    }
}
