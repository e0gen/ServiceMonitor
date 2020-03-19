using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceMonitor.DataAccess;
using ServiceMonitor.Domain.Contracts;
using ServiceMonitor.HttpBusinessServices;

namespace ServiceMonitor.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddHostedService<ServiceChecker>();
                    services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ServiceMonitorDB"),
                        ServiceLifetime.Singleton);
                    services.AddSingleton<IStatusLogService, StatusLogStore>();
                    services.AddSingleton<IStatusProvider, VirtualBusinessService>(s => new VirtualBusinessService("ChuckNorrisService", 0));
                    services.AddSingleton<IStatusProvider, VirtualBusinessService>(s => new VirtualBusinessService("JustNormalService", 0.2m));
                    services.AddSingleton<IStatusProvider, VirtualBusinessService>(s => new VirtualBusinessService("LeeroyJenkinsService", 1));
                });
    }
}
