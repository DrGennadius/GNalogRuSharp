using GNalogRuSharp.Helpers;
using GNalogRuSharp.Models;
using GNalogRuSharp.Services;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GNalogRuSharp.Tests
{
    public class TaxpayerStatusTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1Async()
        {
            TaxpayerStatusService client = new TaxpayerStatusService();
            var result = await client.GetStatusAsync(new TaxpayerStatusData());
            Assert.IsFalse(result.Status);
            Assert.AreEqual(result.Message, "Указан некорректный ИНН: null");

            Assert.Pass();
        }
    }
}