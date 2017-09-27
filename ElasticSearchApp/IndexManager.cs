using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp
{
    public class IndexManager
    {
        public string ElasticSearchUrl { get; }

        private ILogger _logger;

        public IndexManager(string elasticSearchUrl, ILogger logger)
        {
            ElasticSearchUrl = elasticSearchUrl;

            _logger = logger ?? new ESLogger(ElasticSearchUrl);
        }
        public bool CreateIndex(string index)
        {
            var logEntry = new LogEntry
            {
                RequestTime = DateTime.Now.ToString(),
                RequestType = "Create Index",
            };
            try
            {
                var elasticSearchClient = GetEsClient();
                var output = elasticSearchClient.IndexExists(index);
                if (output.Exists == false)
                {
                    var response = elasticSearchClient.CreateIndex(index);
                    logEntry.Status = "Success";
                    logEntry.Response = response.ToString();
                    return true;
                }
                else
                {
                    logEntry.Status = "Failure";
                    logEntry.Response = "Index already exists.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                logEntry.Status = "Failure";
                logEntry.Response = ex.Message;
                return false;
            }
            finally
            {
                _logger.WriteLog(logEntry);
            }
        }

        private ElasticClient GetEsClient()
        {
            var uri = new Uri(ElasticSearchUrl);
            var settings = new ConnectionSettings(uri);
            settings.DisableDirectStreaming();
            return new ElasticClient(settings);
        }

        public bool DeleteIndex(string index)
        {
            var logEntry = new LogEntry
            {
                RequestTime = DateTime.Now.ToString(),
                RequestType = "Delete Index",
            };
            try
            {
                var elasticSearchClient = GetEsClient();
                var response = elasticSearchClient.DeleteIndex(index);
                logEntry.Status = "Success";
                logEntry.Response = response.ToString();

                return true;
            }
            catch (Exception ex)
            {
                logEntry.Status = "Failure";
                logEntry.Response = ex.Message;
                return false;
            }
            finally
            {
                _logger.WriteLog(logEntry);
            }
        }
    }
}
