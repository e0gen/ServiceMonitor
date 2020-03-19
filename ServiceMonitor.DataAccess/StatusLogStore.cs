using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceMonitor.DataAccess.Entities;
using ServiceMonitor.Domain.Contracts;
using ServiceMonitor.Domain.Models;

namespace ServiceMonitor.DataAccess
{
    public class StatusLogStore : IStatusLogService
    {
        private readonly ApplicationDbContext _context;

        public StatusLogStore(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddStatusLog(StatusLog sl)
        {
            var sle = new StatusLogEntity()
            {
                ServiceName = sl.ServiceName,
                Status = sl.Status,
                Date = sl.Date
            };

            _context.StatusLogs.Add(sle);
        }

        public async Task<IList<StatusLog>> GetStatusLogsAsync()
        {
            return await _context.StatusLogs.Select( x => (StatusLog)x)
                .Where(x => x.Date > DateTimeOffset.Now.AddDays(-1))
                .AsNoTracking().ToListAsync();
        }

        public Task<IList<StatusStatistic>> GetStatusStatisticsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
