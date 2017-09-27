using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace ElasticSearchApp
{
    public class ElasticSearchDataStore
    {
        public ElasticSearchDataStore(string elasticSearchUrl, ILogger logger)
        {
            ElasticSearchUrl = elasticSearchUrl;
            _logger = logger ?? new FileSystemLogger();
        }
        private ILogger _logger;
        public string ElasticSearchUrl { get; }

        public List<Hotel> Search(string index, string query)
        {
            List<Hotel> hotelList = new List<Hotel>();
            var logEntry = new LogEntry
            {
                RequestTime = DateTime.Now.ToString(),
                RequestType = "Search",
            };
            try
            {
                var elasticSearchClient = GetEsClient(index);
                var esResponse = elasticSearchClient.Search<Hotel>(s => s
                    .Index(index)
                    .Type("hotel")
                    .Size(100)
                    .Query(
                        q =>
                            q.Match(x => x.Field("name").Query(query))
                    ));

                foreach (var hit in esResponse.Hits)
                {
                    var hotel = new Hotel();
                    hotel.Id = hit.Source.Id;
                    hotel.Name = hit.Source.Name;
                    hotel.Type = hit.Source.Type;
                    hotel.Description = hit.Source.Description;
                    hotelList.Add(hotel);
                }
                logEntry.Status = "Success";
                logEntry.Response = esResponse.ToString();
            }
            catch (Exception e)
            {
                logEntry.Status = "Failure";
                logEntry.Response = e.Message;
            }
            finally
            {
                _logger.WriteLog(logEntry);
            }
            return hotelList;
        }

        public bool AddHotel(string index, Hotel hotel)
        {
            var logEntry = new LogEntry
            {
                RequestTime = DateTime.Now.ToString(),
                RequestType = "AddHotel",
            };
            try
            {
                var elasticSearchClient = GetEsClient(index);
                var esResponse = elasticSearchClient.Index(hotel);
                elasticSearchClient.Refresh(Indices.All);
                logEntry.Status = "Success";
                logEntry.Response = esResponse.ToString();
                return true;
            }
            catch (Exception e)
            {
                logEntry.Status = "Failure";
                logEntry.Response = e.Message;
                return false;
            }
            finally
            {
                _logger.WriteLog(logEntry);
            }
        }

        private ElasticClient GetEsClient(string index)
        {
            var uri = new Uri(ElasticSearchUrl);
            var settings = new ConnectionSettings(uri).DefaultIndex(index);
            settings.DisableDirectStreaming();
            return new ElasticClient(settings);
        }
    }
}
