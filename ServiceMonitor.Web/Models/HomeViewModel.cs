using System.Collections.Generic;
using ServiceMonitor.Domain.Models;

namespace ServiceMonitor.Web.Models
{
    public class HomeViewModel
    {
        public IList<StatusStatistic> StatusStatistics { get; set; } = new List<StatusStatistic>();
    }
}