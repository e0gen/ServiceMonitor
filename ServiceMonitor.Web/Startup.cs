using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceMonitor.DataAccess;
using ServiceMonitor.Domain.Contracts;
using ServiceMonitor.HttpBusinessServices;
using ServiceMonitor.Worker;

namespace ServiceMonitor.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllers();
            services.AddLogging();

            services.AddHostedService<ServiceChecker>();

            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            //    Configuration.GetConnectionString("ServiceMonitorDB")),
            //    ServiceLifetime.Singleton);
            services.AddDbContext<ApplicationDbContext>(options => 
                    options.UseInMemoryDatabase("ServiceMonitorDB"),
                ServiceLifetime.Singleton);
            services.AddSingleton<IStatusLogService, StatusLogStore>();
            
            services.AddSingleton<IStatusProvider, VirtualBusinessService>(s => 
                new VirtualBusinessService("ChuckNorrisService", 0));
            services.AddSingleton<IStatusProvider, VirtualBusinessService>(s => 
                new VirtualBusinessService("JustNormalService", 0.2m));
            services.AddSingleton<IStatusProvider, VirtualBusinessService>(s => 
                new VirtualBusinessService("LeeroyJenkinsService", 1));
            services.AddSingleton<IStatusProvider, ExternalBusinessService>(s => 
                new ExternalBusinessService("[External] ToDoService",
                    "https://jsonplaceholder.typicode.com/todos/1", new HttpClient()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
