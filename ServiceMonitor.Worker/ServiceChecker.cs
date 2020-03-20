using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Domain.Contracts;
using ServiceMonitor.Domain.Models;

namespace ServiceMonitor.Worker
{
    public class ServiceChecker : BackgroundService
    {
        private readonly int _delay;
        private readonly ILogger<ServiceChecker> _logger;
        private readonly IEnumerable<IStatusProvider>  _businessServices;
        private readonly IStatusLogService _storeService;
        public ServiceChecker(
            ILogger<ServiceChecker> logger,
            IEnumerable<IStatusProvider> businessServices, 
            IStatusLogService storeService,
            int delay = 5 * 1000)
        {
            _logger = logger;
            _businessServices = businessServices;
            _storeService = storeService;
            _delay = delay;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("ServiceChecker tracking {count} services at: {time}", 
                    _businessServices.Count(), DateTimeOffset.Now);

                var listOfTasks = new List<(Task<HttpResponseMessage> Task, string Name)>();

                foreach (var businessService in _businessServices)
                {
                    listOfTasks.Add((businessService.GetStatusAsync(),  businessService.ServiceName));
                }

                //await Task.WhenAll(listOfTasks);

                foreach (var task in listOfTasks)
                {
                    var result = await task.Task;
                    var log = new StatusLog()
                    {
                        ServiceName = task.Name,
                        Status = result.StatusCode,
                        Date = result.Headers.Date ?? DateTimeOffset.Now
                    };
                    _storeService.AddStatusLog(log);
                }
                await _storeService.SaveChangesAsync();

                await Task.Delay(_delay, stoppingToken);
            }
        }
    }
}
