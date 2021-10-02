using GNalogRuSharp.Helpers;
using GNalogRuSharp.Services;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace GNalogRuSharp.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            InnService client = new InnService();
            bool isSucces = client.FetchINN();
            Assert.IsFalse(isSucces);

            client.SetData("Фамилия", "Имя", "Отчество", null, DocumentType.PassportRussia, "9414 435125");
            isSucces = client.FetchINN();
            Assert.IsFalse(isSucces);

            client.RemoteCertificateValidationCallbackFunc = TestValidationCallback;
            isSucces = client.FetchINN();
            Assert.IsFalse(isSucces);

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