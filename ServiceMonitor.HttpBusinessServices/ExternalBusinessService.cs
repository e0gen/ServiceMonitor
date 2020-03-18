using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceMonitor.Domain.Contracts;

namespace ServiceMonitor.HttpBusinessServices
{
    public class ExternalBusinessService : IStatusProvider
    {
        private readonly string _serviceName;
        private readonly HttpClient _client;
        private readonly string _url;

        public ExternalBusinessService(string serviceName, string url, HttpClient client)
        {
            if (string.IsNullOrEmpty(serviceName)) throw new ArgumentException(nameof(serviceName));

            _serviceName = serviceName;
            _client = client;
            _url = url;
        }

        public virtual string ServiceName => _serviceName;
        public virtual Task<HttpResponseMessage> GetStatusAsync()
        {
            return _client.GetAsync(_url);
        }
    }
}
