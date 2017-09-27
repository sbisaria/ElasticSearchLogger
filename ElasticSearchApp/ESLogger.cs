using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp
{
    public class ESLogger : ILogger
    {
        private ElasticClient elasticSearchClient;

 
        public ESLogger(string elasticSearchUrl)
        {
            var uri = new Uri(elasticSearchUrl);
            var settings = new ConnectionSettings(uri).DefaultIndex("logs");
            settings.DisableDirectStreaming();
            elasticSearchClient = new ElasticClient(settings);
        }
        public void WriteLog(LogEntry logEntry)
        {
            elasticSearchClient.Index(logEntry);
        }
        public void Writelog(LogEntry logEntry,string index)
        {
            elasticSearchClient.Index(logEntry, x => x.Index(index).Type("log").Refresh(Elasticsearch.Net.Refresh.True));
        }
    }
}
