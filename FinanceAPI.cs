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
        /// <summary>
        /// Метод получения данных для построения свечного графика
        /// </summary>
        public async Task<string> GetHistoricalData(string symbol, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Формируем URL запроса с нужными параметрами для MOEX API
                string url = $"history/engines/stock/markets/shares/boards/TQBR/securities/{symbol}.json?iss.meta=off&from={startDate:yyyy-MM-dd}&till={endDate:yyyy-MM-dd}";

                // Отправляем GET-запрос и получаем ответ
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                // Проверяем успешность запроса
                if (response.IsSuccessStatusCode)
                {
                    // Читаем содержимое ответа
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Возвращаем данные (в зависимости от формата ответа)
                    return responseData;
                }
                else
                {
                    // Обработка ошибки запроса
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений, если они возникнут
                return $"Exception: {ex.Message}";
            }
        }
    } 
}

