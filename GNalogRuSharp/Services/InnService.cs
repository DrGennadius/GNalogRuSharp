using GNalogRuSharp.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GNalogRuSharp.Services
{
    /// <summary>
    /// Неофицальный клиент для выполнения запросов на сервис налоговой службы для получения ИНН по паспортным данным.
    /// </summary>
    public class InnService
    {
        private bool _isUseDefaultCertificateValidation = false;

        private RemoteCertificateValidationCallback _remoteCertificateValidationCallbackFunc = null;

        public InnService(string apiUrl)
        {
            ApiUrl = apiUrl;
        }

        public InnService()
        {
        }

        /// <summary>
        /// Адрес запроса.
        /// </summary>
        public string ApiUrl { get; } = "https://service.nalog.ru/inn-proc.do";

        /// <summary>
        /// Использовать стандартную реализацию колбека с проверкой сертификата установленного в системе и с сайта.
        /// </summary>
        public bool IsUseDefaultCertificateValidation 
        { 
            get => _isUseDefaultCertificateValidation; 
            set
            {
                _isUseDefaultCertificateValidation = value;
                if (value)
                {
                    _remoteCertificateValidationCallbackFunc = null;
                }
            }
        }

        /// <summary>
        /// Свой колбек.
        /// </summary>
        public RemoteCertificateValidationCallback RemoteCertificateValidationCallbackFunc
        {
            get => _remoteCertificateValidationCallbackFunc;
            set
            {
                _remoteCertificateValidationCallbackFunc = value;
                _isUseDefaultCertificateValidation = false;
            }
        }

        /// <summary>
        /// Данные в виде строки для запроса
        /// </summary>
        public string DataString { get; private set; }

        /// <summary>
        /// Строка ошибки
        /// </summary>
        public string ErrorString { get; private set; }

        /// <summary>
        /// Строка ответа
        /// </summary>
        /// <example>
        /// 1. Пример ответа API налоговой, если ИНН найден:
        /// {'inn': 'xxxxxxxxxxxx', 'captchaRequired': False, 'code': 1}
        /// 2. Ответ API, если ИНН не найден:
        /// {'captchaRequired': False, 'code': 0}
        /// </example>
        public string ResponseString { get; private set; }

        /// <summary>
        /// Полученная информация.
        /// </summary>
        public InnResult FNSInfo { get; private set; }

        /// <summary>
        /// Установить данные.
        /// </summary>
        /// <param name="surname">Фамилия (обязательно)</param>
        /// <param name="name">Имя (обязательно)</param>
        /// <param name="patronymic">Отчество (обязательно)</param>
        /// <param name="birthDate">Дата рождения (обязательно)</param>
        /// <param name="docType">Вид документа, удостоверяющего личность (обязательно)</param>
        /// <param name="docNumber">Серия и номер документа (обязательно)</param>
        /// <param name="birthPlace">Место рождения</param>
        /// <param name="docDate">Дата выдачи документа</param>
        public void SetData(string surname, string name, string patronymic, DateTime? birthDate, DocumentType docType, string docNumber, string birthPlace = null, DateTime? docDate = null)
        {
            InnData innData = new InnData()
            {
                Surname = surname,
                Name = name,
                Patronymic = patronymic,
                BirthDate = birthDate,
                DocType = docType,
                DocNumber = docNumber,
                BirthPlace = birthPlace,
                DocDate = docDate
            };
            DataString = innData.UrlEncode();
        }

        /// <summary>
        /// Сделать запрос на получение ИНН.
        /// </summary>
        /// <returns></returns>
        public bool FetchINN()
        {
            FNSInfo = new InnResult()
            {
                Code = 0
            };

            if (string.IsNullOrWhiteSpace(DataString))
            {
                return false;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);

            // Устанавливаем колбек валидации сертификата
            if (_isUseDefaultCertificateValidation)
            {
                request.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
            }
            else if (_remoteCertificateValidationCallbackFunc != null)
            {
                request.ServerCertificateValidationCallback += _remoteCertificateValidationCallbackFunc;
            }

            // Для отправки используется метод Post
            request.Method = "POST";

            // Преобразуем данные в массив байтов
            byte[] byteArray = Encoding.UTF8.GetBytes(DataString);
            // устанавливаем тип содержимого - параметр ContentType
            request.ContentType = "application/x-www-form-urlencoded";
            // Устанавливаем заголовок Content-Length запроса - свойство ContentLength
            request.ContentLength = byteArray.Length;

            // Записываем данные в поток запроса
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(FNSInfo));
                        //FNSInfo = (FNSInfo)deserializer.ReadObject(stream);
                        ResponseString = reader.ReadToEnd();
                        FNSInfo = JsonConvert.DeserializeObject<InnResult>(ResponseString);
                    }
                }
                response.Close();
            }
            catch (WebException webEx)
            {
                if (webEx.Response != null)
                {
                    using (var stream = webEx.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            ErrorString = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorString = ex.Message;
            }

            return FNSInfo.Code == 1;
        }

        private bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                return false;
            }

            X509Certificate2 cert2 = new X509Certificate2(certificate);

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            var collection = store.Certificates.Find(X509FindType.FindBySerialNumber, cert2.SerialNumber, true);
            store.Close();

            // сверка сертификатов
            return collection != null && collection.Count > 0 && collection[0].Equals(cert2);
        }
    }
}
