using System;
using System.Text;

namespace GNalogRuSharp.Models
{
    /// <summary>
    /// Данные для отправки в сервис для получения ИНН по паспортным данным.
    /// </summary>
    public class InnData
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime? BirthDate { get; set; }
        public DocumentType DocType { get; set; }
        public string DocNumber { get; set; }
        public string BirthPlace { get; set; }
        public DateTime? DocDate { get; set; }

        /// <summary>
        /// Кодирование/преобразование данных в строку, пригодную для передачи.
        /// </summary>
        /// <returns></returns>
        public string UrlEncode()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(PropertyUrlEncode("c", "innMy", stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("fam", Surname, stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("nam", Name, stringBuilder.Length));
            if (!string.IsNullOrEmpty(Patronymic))
            {
                stringBuilder.Append(PropertyUrlEncode("otch", Patronymic, stringBuilder.Length));
            }
            else
            {
                // opt_otch=1 - это когда не нужно отчество, чтобы не требовали.
                stringBuilder.Append(PropertyUrlEncode("opt_otch", "1", stringBuilder.Length));
            }
            int docType = (int)DocType;
            string birthDateString = BirthDate.HasValue ? BirthDate.Value.ToShortDateString() : "";
            string docDateString = DocDate.HasValue ? DocDate.Value.ToShortDateString() : "";
            stringBuilder.Append(PropertyUrlEncode("bdate", birthDateString, stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("bplace", BirthPlace, stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("doctype", docType.ToString("00"), stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("docno", DocNumber, stringBuilder.Length));
            stringBuilder.Append(PropertyUrlEncode("docdt", docDateString, stringBuilder.Length));

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
    }
}
