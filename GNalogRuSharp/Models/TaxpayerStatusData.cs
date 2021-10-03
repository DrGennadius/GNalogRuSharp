using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNalogRuSharp.Models
{
    /// <summary>
    /// Данные для отправки в сервис "Проверка статуса налогоплательщика налога на профессиональный доход(самозанятого)"
    /// </summary>
    public class TaxpayerStatusData
    {
        public TaxpayerStatusData()
        {
            SetRequestDate(DateTime.Now);
        }

        public TaxpayerStatusData(string inn)
        {
            Inn = inn;
            SetRequestDate(DateTime.Now);
        }

        public TaxpayerStatusData(string inn, string requestDate)
        {
            Inn = inn;
            RequestDate = requestDate;
        }

        public TaxpayerStatusData(string inn, DateTime requestDate)
        {
            Inn = inn;
            SetRequestDate(requestDate);
        }

        public TaxpayerStatusData(string inn, DateTime? requestDate)
        {
            Inn = inn;
            if (requestDate.HasValue)
            {
                SetRequestDate(requestDate.Value);
            }
        }

        /// <summary>
        /// ИНН налогоплательщика.
        /// </summary>
        [JsonProperty("inn")]
        public string Inn { get; set; }

        /// <summary>
        /// Дата, для которой будет осуществлена проверка статуса самозанятого.
        /// </summary>
        [JsonProperty("requestDate")]
        public string RequestDate { get; set; }

        /// <summary>
        /// Установка даты.
        /// </summary>
        /// <param name="dateTime"></param>
        public void SetRequestDate(DateTime dateTime) => RequestDate = dateTime.ToString("yyyy-MM-dd");
    }
}
