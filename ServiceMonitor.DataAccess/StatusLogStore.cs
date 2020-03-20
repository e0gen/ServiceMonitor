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

        public async Task<IList<StatusStatistic>> GetStatusStatisticsAsync()
        {
            var date1HourAgo= DateTimeOffset.Now.AddHours(-1);
            var date1DayAgo = DateTimeOffset.Now.AddDays(-1);
            var baseQuery = _context.StatusLogs.Where(x => x.Date >= date1DayAgo);

            var resultQuery = from o in baseQuery
                group o by o.ServiceName into g
                select new StatusStatistic()
                {
                    ServiceName = g.Key,
                    LastStatus = baseQuery.Where(x => x.ServiceName == g.Key).Select(x => x.Status).Last(),
                    LastHourFails = g.Sum(d => d.Date >= date1HourAgo && !((int)d.Status >= 200 && (int)d.Status < 300) ? 1 : 0),
                    LastDayFails = g.Sum(d => !((int)d.Status >= 200 && (int)d.Status < 300) ? 1 : 0)
                };

            return await resultQuery.AsNoTracking().ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
