using System.Runtime.Serialization;

namespace GNalogRuSharp
{
    /// <summary>
    /// Данные, возвращаемые непосредственно после запроса.
    /// </summary>
    [DataContract]
    public class FNSInfo
    {
        /// <summary>
        /// ИНН
        /// </summary>
        /// <example>
        /// xxxxxxxxxxxx
        /// </example>
        [DataMember(Name = "inn")]
        public string INN { get; set; }

        /// <summary>
        /// Требуется Captcha (?)
        /// </summary>
        /// <remarks>
        /// Видимо, если нужна будет решать капчу.
        /// Не известно что будет, если все таки такое произойдет.
        /// </remarks>
        [DataMember(Name = "captchaRequired")]
        public bool CaptchaRequired { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        /// <example>
        /// 1 если ИНН найден. 0 - не найден.
        /// </example>
        [DataMember(Name = "code")]
        public int Code { get; set; }
    }
}
