using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceMonitor.Domain.Models;

namespace ServiceMonitor.Domain.Contracts
{
    public interface IStatusLogService
    {
        void AddStatusLog(StatusLog sl);
        Task SaveChangesAsync();
        Task<IList<StatusLog>> GetStatusLogsAsync();
        Task<IList<StatusStatistic>> GetStatusStatisticsAsync();
    }
}
