using ServiceMonitor.Domain.Models;

namespace ServiceMonitor.DataAccess.Entities
{
    public class StatusLogEntity : StatusLog
    {
        public long StatusLogEntityId { get; set; }
    }
}
