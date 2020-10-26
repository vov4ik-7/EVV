using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace WebAdvertisements.SearchWorker
{
    public static class ElasticSearchHelper
    {
        private static IElasticClient _client;

        public static IElasticClient GetInstance()
        {
            if (_client == null)
            {
                var url = "https://search-advertapi-n5fn74stkps6pv4etvybd6z5ly.eu-central-1.es.amazonaws.com";
                var settings = new ConnectionSettings(new Uri(url))
                    .DefaultIndex("adverts")
                    .DefaultTypeName("adverts")
                    .DefaultMappingFor<AdvertType>(m => m.IdProperty(x => x.Id));
                _client = new ElasticClient(settings);
            }

            return _client;
        }
    }
}
