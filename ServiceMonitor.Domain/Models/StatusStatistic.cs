using System.Net;

namespace ServiceMonitor.Domain.Models
{
    public class StatusStatistic
    {
        public string ServiceName { get; set; }
        public HttpStatusCode LastStatus { get; set; }
        public int LastHourFails { get; set; }
        public int LastDayFails { get; set; }
    }
}
