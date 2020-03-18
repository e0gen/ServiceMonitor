using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceMonitor.HttpBusinessServices;
using System.Net.Http;

namespace ServiceMonitor.Tests
{
    [TestFixture]
    class BusinessServicesTests
    {
        [TestCase("")]
        [TestCase(null)]
        public void VirtualBusinessService_BadArgumentFail(string arg)
        {
            //Assert
            Assert.Catch<ArgumentException>(() => new VirtualBusinessService(arg));
        }

        [TestCase(0)]
        [TestCase(0.75)]
        [TestCase(0.5)]
        [TestCase(0.25)]
        [TestCase(1)]
        public async Task VirtualBusinessService_Work(decimal expectedErrorRatio)
        {
            // Arrange
            int checks = 1000;
            var sut = new VirtualBusinessService("Name", expectedErrorRatio);
            var list = new List<HttpResponseMessage>();

            // Act
            for (int i = 0; i < checks; i++)
            {
                list.Add(await sut.GetStatusAsync());
            }

            var failList = list.Where(x => !x.IsSuccessStatusCode).Select(x => x).ToList();
            decimal realErrorRatio = (decimal)failList.Count / checks;
            //Assert
            Assert.LessOrEqual(Math.Round(realErrorRatio - expectedErrorRatio, 2), 0.02m);
        }
    }
}
