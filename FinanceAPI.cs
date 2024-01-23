using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;

namespace InvestmentAssistant
{
    public class FinanceAPI
    {
        private readonly HttpClient _httpClient;

        public FinanceAPI() 
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://iss.moex.com/iss/");

        }
        /// <summary>
        /// Метод получения списка бумаг на московской бирже
        /// </summary>
        public async Task<List<NameOfSecurities>> GetListOfSecurities()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("engines/stock/markets/shares/securities.json");
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            // Обработка JSON и извлечение нужных полей
            JObject json = JObject.Parse(jsonResponse);
            var securities = json["securities"]["data"].Select(security =>
            {
                string secId = security[0]?.ToString() ?? "";
                string secName = security[9]?.ToString() ?? "";            

                return new NameOfSecurities
                {
                    SecurityId = secId,
                    SecurityName = secName
                };
            }).ToList();
            return securities;
        }
    } 
}

