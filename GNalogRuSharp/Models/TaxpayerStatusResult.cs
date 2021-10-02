using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNalogRuSharp.Models
{
    /// <summary>
    /// Данные возвращаемые с сервиса "Проверка статуса налогоплательщика налога на профессиональный доход(самозанятого)"
    /// </summary>
    public class TaxpayerStatusResult
    {
        /// <summary>
        /// Является ли указанный ИНН плательщиком налога на профессиональный доход.
        /// </summary>
        [JsonProperty("status")]
        public bool Status { get; set; }

        /// <summary>
        /// Сообщение с ответом сервиса.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
