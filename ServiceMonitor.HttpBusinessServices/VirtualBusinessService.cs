using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceMonitor.Domain.Contracts;

namespace ServiceMonitor.HttpBusinessServices
{
    public class VirtualBusinessService : IStatusProvider
    {
        protected readonly string _serviceName;
        private readonly decimal _errorRatio;

        public VirtualBusinessService(string serviceName, decimal errorRatio = 0)
        {
            if (string.IsNullOrEmpty(serviceName)) throw new ArgumentException(nameof(serviceName));

            _serviceName = serviceName;
            _errorRatio = errorRatio;
        }

        public virtual string ServiceName => _serviceName;
        public virtual Task<HttpResponseMessage> GetStatusAsync()
        {
            return Task.Factory.StartNew(() =>
                new HttpResponseMessage(
                    IsRequestFail() ? HttpStatusCode.BadRequest : HttpStatusCode.OK
                    ));
        }

        private bool IsRequestFail()
        {
            Random r = new Random();
            if (r.Next(0, 1000) < 1000 * _errorRatio)
                return true;
            return false;
        }
    }
}
