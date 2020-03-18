using System;
using System.Net;

namespace ServiceMonitor.Domain.Models
{
    public class StatusLog
    {
        public string ServiceName { get; set; }
        public HttpStatusCode Status { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
