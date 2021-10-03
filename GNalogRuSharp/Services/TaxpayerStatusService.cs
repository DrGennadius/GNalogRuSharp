using GNalogRuSharp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GNalogRuSharp.Services
{
    /// <summary>
    /// Проверка статуса налогоплательщика налога на профессиональный доход(самозанятого)
    /// </summary>
    public class TaxpayerStatusService
    {
        /// <summary>
        /// Адрес запроса.
        /// </summary>
        public string ApiUrl { get; } = "https://statusnpd.nalog.ru/api/v1/tracker/taxpayer_status";

        /// <summary>
        /// Получить информацию о статусе налогоплательщика налога на профессиональный доход(самозанятого)
        /// </summary>
        /// <param name="taxpayerStatusData">Данные для отправки.</param>
        /// <returns></returns>
        public async Task<TaxpayerStatusResult> GetStatusAsync(TaxpayerStatusData taxpayerStatusData)
        {
            TaxpayerStatusResult result = new TaxpayerStatusResult();

            var dataString = await Task.Run(() => JsonConvert.SerializeObject(taxpayerStatusData));
            var httpContent = new StringContent(dataString, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.PostAsync(ApiUrl, httpContent);

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    result = await Task.Run(() => JsonConvert.DeserializeObject<TaxpayerStatusResult>(responseContent));
                }
            }

            return result;
        }

        /// <summary>
        /// Получить информацию о статусе налогоплательщика налога на профессиональный доход(самозанятого)
        /// </summary>
        /// <param name="inn">ИНН налогоплательщика.</param>
        /// <param name="requestDate">Дата, для которой будет осуществлена проверка статуса самозанятого.</param>
        /// <returns></returns>
        public async Task<TaxpayerStatusResult> GetStatusAsync(string inn, DateTime requestDate)
        {
            TaxpayerStatusData taxpayerStatusData = new TaxpayerStatusData(inn, requestDate);
            return await GetStatusAsync(taxpayerStatusData);
        }

        /// <summary>
        /// Получить информацию о статусе налогоплательщика налога на профессиональный доход(самозанятого)
        /// </summary>
        /// <param name="inn">ИНН налогоплательщика.</param>
        /// <param name="requestDate">Дата, для которой будет осуществлена проверка статуса самозанятого.</param>
        /// <returns></returns>
        public async Task<TaxpayerStatusResult> GetStatusAsync(string inn, DateTime? requestDate)
        {
            TaxpayerStatusData taxpayerStatusData = new TaxpayerStatusData(inn, requestDate);
            return await GetStatusAsync(taxpayerStatusData);
        }

        /// <summary>
        /// Получить за сегодня информацию о статусе налогоплательщика налога на профессиональный доход(самозанятого)
        /// </summary>
        /// <param name="inn">ИНН налогоплательщика.</param>
        /// <returns></returns>
        public async Task<TaxpayerStatusResult> GetStatusAsync(string inn)
        {
            TaxpayerStatusData taxpayerStatusData = new TaxpayerStatusData(inn);
            return await GetStatusAsync(taxpayerStatusData);
        }
    }
}
