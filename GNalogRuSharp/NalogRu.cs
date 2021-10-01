using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GNalogRuSharp
{
    /// <summary>
    /// Неофицальный клиент для выполнения запросов на сервис налоговой службы.
    /// </summary>
    public class NalogRu
    {
        private bool _isUseDefaultCertificateValidation = false;

        private RemoteCertificateValidationCallback _remoteCertificateValidationCallbackFunc = null;

        public NalogRu(string apiUrl)
        {
            ApiUrl = apiUrl;
        }

        public NalogRu()
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
        public FNSInfo FNSInfo { get; private set; }

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
            DataString = UrlEncode(surname, name, patronymic, birthDate, docType, docNumber, birthPlace, docDate);
        }

        /// <summary>
        /// Сделать запрос на получение ИНН.
        /// </summary>
        /// <returns></returns>
        public bool FetchINN()
        {
            FNSInfo = new FNSInfo()
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
                        FNSInfo = JsonConvert.DeserializeObject<FNSInfo>(ResponseString);
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

        /// <summary>
        /// Кодирование/преобразование данных в строку, пригодную для передачи.
        /// </summary>
        /// <param name="surname">Фамилия (обязательно)</param>
        /// <param name="name">Имя (обязательно)</param>
        /// <param name="patronymic">Отчество (обязательно)</param>
        /// <param name="birthDate">Дата рождения (обязательно)</param>
        /// <param name="docType">Вид документа, удостоверяющего личность (обязательно)</param>
        /// <param name="docNumber">Серия и номер документа (обязательно)</param>
        /// <param name="birthPlace">Место рождения</param>
        /// <param name="docDate">Дата выдачи документа</param>
        /// <returns></returns>
        private string UrlEncode(string surname, string name, string patronymic, DateTime? birthDate, DocumentType docType, string docNumber, string birthPlace = null, DateTime? docDate = null)
        {
            return UrlEncode(surname, name, patronymic, birthDate, (int)docType, docNumber, birthPlace, docDate);
        }

        /// <summary>
        /// Кодирование/преобразование данных в строку, пригодную для передачи.
        /// </summary>
        /// <param name="surname">Фамилия (обязательно)</param>
        /// <param name="name">Имя (обязательно)</param>
        /// <param name="patronymic">Отчество (обязательно)</param>
        /// <param name="birthDate">Дата рождения (обязательно)</param>
        /// <param name="docType">Вид документа, удостоверяющего личность (обязательно)</param>
        /// <param name="docNumber">Серия и номер документа (обязательно)</param>
        /// <param name="birthPlace">Место рождения</param>
        /// <param name="docDate">Дата выдачи документа</param>
        /// <returns></returns>
        private string UrlEncode(string surname, string name, string patronymic, DateTime? birthDate, int docType, string docNumber, string birthPlace = null, DateTime? docDate = null)
        {
            return UrlEncode(surname, name, patronymic, birthDate, docType.ToString("00"), docNumber, birthPlace, docDate);
        }

        /// <summary>
        /// Кодирование/преобразование данных в строку, пригодную для передачи.
        /// </summary>
        /// <param name="surname">Фамилия (обязательно)</param>
        /// <param name="name">Имя (обязательно)</param>
        /// <param name="patronymic">Отчество (обязательно)</param>
        /// <param name="birthDate">Дата рождения (обязательно)</param>
        /// <param name="docType">Вид документа, удостоверяющего личность (обязательно)</param>
        /// <param name="docNumber">Серия и номер документа (обязательно)</param>
        /// <param name="birthPlace">Место рождения</param>
        /// <param name="docDate">Дата выдачи документа</param>
        /// <returns></returns>
        private string UrlEncode(string surname, string name, string patronymic, DateTime? birthDate, string docType, string docNumber, string birthPlace = null, DateTime? docDate = null)
        {
            string birthDateString = birthDate.HasValue ? birthDate.Value.ToShortDateString() : null;
            string docDateString = docDate.HasValue ? docDate.Value.ToShortDateString() : null;
            return UrlEncode(surname, name, patronymic, birthDateString, docType, docNumber, birthPlace, docDateString);
        }

        /// <summary>
        /// Кодирование/преобразование данных в строку, пригодную для передачи.
        /// </summary>
        /// <param name="surname">Фамилия (обязательно)</param>
        /// <param name="name">Имя (обязательно)</param>
        /// <param name="patronymic">Отчество (обязательно)</param>
        /// <param name="birthDate">Дата рождения (обязательно)</param>
        /// <param name="docType">Вид документа, удостоверяющего личность (обязательно)</param>
        /// <param name="docNumber">Серия и номер документа (обязательно)</param>
        /// <param name="birthPlace">Место рождения</param>
        /// <param name="docDate">Дата выдачи документа</param>
        /// <returns></returns>
        private string UrlEncode(string surname, string name, string patronymic, string birthDate, string docType, string docNumber, string birthPlace = null, string docDate = null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(PropertyUrlEncode("c", "innMy", stringBuilder.Length));

            stringBuilder.Append(PropertyUrlEncode("fam", surname, stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("nam", name, stringBuilder.Length));
            if (!string.IsNullOrEmpty(patronymic))
            {
                stringBuilder.Append(PropertyUrlEncode("otch", patronymic, stringBuilder.Length));
            }
            else
            {
                // opt_otch=1 - это когда не нужно отчество, чтобы не требовали.
                stringBuilder.Append(PropertyUrlEncode("opt_otch", "1", stringBuilder.Length));
            }
            stringBuilder.Append(PropertyUrlEncode("bdate", birthDate, stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("bplace", birthPlace, stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("doctype", docType, stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("docno", docNumber, stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("docdt", docDate, stringBuilder.Length));

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Преобразование параметра [name-value] в строку, пригодную для передачи.
        /// </summary>
        /// <param name="name">Имя параметра</param>
        /// <param name="val">Значение параметра</param>
        /// <param name="readyLength">Длина уже готовой строки данных</param>
        /// <returns></returns>
        private string PropertyUrlEncode(string name, string val, int readyLength = 0)
        {
            if (string.IsNullOrEmpty(val))
            {
                return "";
            }
            //TODO: Если что, то тут можно всякие другие преобразования делать.
            string result = name + '=' + val;
            return readyLength > 0 ? '&' + result : result;
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
