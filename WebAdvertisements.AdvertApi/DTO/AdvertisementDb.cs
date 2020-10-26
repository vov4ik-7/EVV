using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using WebAdvertisements.AdvertApi.Models;

namespace WebAdvertisements.AdvertApi.DTO
{
    [DynamoDBTable("Advertisements")]
    public class AdvertisementDb
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public double Price { get; set; }

        [DynamoDBProperty]
        public DateTime CreationTime { get; set; }

        [DynamoDBProperty]
        public AdvertisementStatus Status { get; set; }

        [DynamoDBProperty]
        public string FilePath { get; set; }

        [DynamoDBProperty]
        public string UserName { get; set; }
    }
}
