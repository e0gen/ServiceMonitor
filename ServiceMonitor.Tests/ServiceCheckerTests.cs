using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ServiceMonitor.Domain.Contracts;
using ServiceMonitor.Domain.Models;
using ServiceMonitor.Worker;

namespace ServiceMonitor.Tests
{
    class ServiceCheckerTests
    {
        [Test]
        public async Task ServiceChecker_Work()
        {
            // Arrange
            var logger = new Mock<ILogger<ServiceChecker>>();
            var storeService = new Mock<IStatusLogService>();

            var service1 = new Mock<IStatusProvider>();
            service1
                .Setup(x => x.GetStatusAsync())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var service2 = new Mock<IStatusProvider>();
            service2
                .Setup(x => x.GetStatusAsync())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

            var sut = new ServiceChecker(logger.Object, new List<IStatusProvider>()
                {service1.Object, service2.Object}, storeService.Object);
            // Act
            var cts = new CancellationTokenSource();

            await sut.StartAsync(cts.Token);

            await Task.Delay(1000);
            cts.Cancel();

            //Assert
            service1.Verify(m => m.GetStatusAsync(), Times.Once);
            service2.Verify(m => m.GetStatusAsync(), Times.Once);
            storeService.Verify(m => m.AddStatusLog(It.IsAny<StatusLog>()), Times.Exactly(2));
            storeService.Verify(m => m.SaveChangesAsync(), Times.Once);
        }
    }
}
