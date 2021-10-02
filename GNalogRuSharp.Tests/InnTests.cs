using GNalogRuSharp.Helpers;
using GNalogRuSharp.Services;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GNalogRuSharp.Tests
{
    public class InnTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1Async()
        {
            InnService client = new InnService();

            var result = await client.GetInnAsync("Фамилия", "Имя", "Отчество", null, DocumentType.PassportRussia, "9414 435125");
            Assert.AreEqual(result.Code, 1);

            client.RemoteCertificateValidationCallbackFunc = TestValidationCallback;
            result = await client.GetInnAsync("Фамилия", "Имя", "Отчество", null, DocumentType.PassportRussia, "9414 435125");
            Assert.AreEqual(result.Code, 1);

            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            var docTypeValueAndDescription = EnumHelper.GetAllValuesAndDescriptions(typeof(DocumentType));
            Assert.IsTrue(docTypeValueAndDescription.Any());

            Assert.Pass();
        }

        private bool TestValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(certificate.Subject);

            X509Certificate2 cert = new X509Certificate2(certificate);

            return cert.Verify();
        }
    }
}