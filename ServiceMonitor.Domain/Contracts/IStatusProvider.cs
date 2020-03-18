using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceMonitor.Domain.Contracts
{
    public interface IStatusProvider
    {
        string ServiceName { get; }
        Task<HttpResponseMessage> GetStatusAsync();
    }
}