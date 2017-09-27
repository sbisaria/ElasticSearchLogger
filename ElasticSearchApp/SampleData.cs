using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp
{
    public class SampleData
    {
        private ElasticClient elasticSearchClient;
        string ElasticSearchUrl;
        public SampleData(string elasticSearchUrl)
        {
            ElasticSearchUrl = elasticSearchUrl;
        }

        public void PopulateSampleDataInEs(string index)
        {
            var uri = new Uri(ElasticSearchUrl);
            var settings = new ConnectionSettings(uri).DefaultIndex(index);
            settings.DisableDirectStreaming();
            elasticSearchClient = new ElasticClient(settings);
            foreach (var hotel in GetData())
            {
                elasticSearchClient.Index(hotel);
            }
        }

        private List<Hotel> GetData()
        {
            return new List<Hotel> {
                new Hotel {Id = 1, Name = "Hyatt",Type="Hotel", Description = "A Five Star Hotel"},
                new Hotel {Id = 2, Name = "Conrad",Type="Hotel", Description = "Luxury Hotel"},
                new Hotel {Id = 3, Name = "Holiday-In",Type="Hotel", Description = "A Five Star Hotel"},
                new Hotel {Id = 4, Name = "Country-In",Type="Hotel", Description = "Luxury Hotel"},
                new Hotel {Id = 5, Name = "Clarks-In",Type="Hotel", Description = "Luxury Hotel"}
            };
        }
    }
}
