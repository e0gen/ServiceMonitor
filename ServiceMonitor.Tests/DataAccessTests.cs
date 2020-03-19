using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ServiceMonitor.DataAccess;
using ServiceMonitor.DataAccess.Entities;
using ServiceMonitor.Domain.Models;

namespace ServiceMonitor.Tests
{
    [TestFixture]
    class DataAccessTests
    {
        [Test]
        public async Task AddStatusLog_Work()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddItemsToDb")
                .Options;
            var newEntry = new StatusLog()
            {
                ServiceName = "Service1",
                Status = HttpStatusCode.OK,
                Date = DateTimeOffset.Now
            };
            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var sut = new StatusLogStore(context);
                

                sut.AddStatusLog(newEntry);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                Assert.AreEqual(1, context.StatusLogs.Count());
                Assert.AreEqual( newEntry.ServiceName, context.StatusLogs.First().ServiceName);
            }
        }

        private DbContextOptions<ApplicationDbContext> options_global = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "FindSearches")
            .Options;

        [OneTimeSetUp]
        public void DbSetUp()
        {
            //Service 1 currently works fine, but had an errors few days ago
            //Service 2 currently previously works fine, but have few errors today
            //10 entries in total, but only 6 were happened in last 24 hours

            // Act
            using var context = new ApplicationDbContext(options_global);
            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service1", Status = HttpStatusCode.OK, Date = DateTimeOffset.Now });
            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service1", Status = HttpStatusCode.OK, Date = DateTimeOffset.Now.AddMinutes(-30) });
            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service1", Status = HttpStatusCode.OK, Date = DateTimeOffset.Now.AddHours(-2) });
            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service1", Status = HttpStatusCode.BadRequest, Date = DateTimeOffset.Now.AddDays(-2) });
            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service1", Status = HttpStatusCode.BadRequest, Date = DateTimeOffset.Now.AddDays(-3) });

            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service2", Status = HttpStatusCode.OK, Date = DateTimeOffset.Now });
            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service2", Status = HttpStatusCode.BadRequest, Date = DateTimeOffset.Now.AddMinutes(-30) });
            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service2", Status = HttpStatusCode.BadRequest, Date = DateTimeOffset.Now.AddHours(-2) });
            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service2", Status = HttpStatusCode.OK, Date = DateTimeOffset.Now.AddDays(-2) });
            context.StatusLogs.Add(new StatusLogEntity()
            { ServiceName = "Service2", Status = HttpStatusCode.OK, Date = DateTimeOffset.Now.AddDays(-3) });
            context.SaveChanges();
        }


        [Test]
        public async Task GetStatusLogsAsync_Work()
        {
            // Assert
            using (var context = new ApplicationDbContext(options_global))
            {
                var sut = new StatusLogStore(context);
                var result = await sut.GetStatusLogsAsync();
                Assert.AreEqual(6, result.Count);
            }
        }
    }
}
