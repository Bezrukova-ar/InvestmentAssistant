using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;

namespace InvestmentAssistant
{

    /// <summary>
    /// Класс FinanceAPI инкапсулирует функциональные возможности
    /// взаимодействия с API Московской биржи(MOEX) 
    /// для получения информации о ценных бумагах
    /// </summary>
    public class FinanceAPI
    {
        private readonly HttpClient _httpClient;

        public FinanceAPI() 
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://iss.moex.com/iss/");

        }

        /// <summary> Метод получения списка бумаг на московской бирже </summary>
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

        /// <summary> Метод получения данных для построения свечного графика </summary>
        public async Task<List<CandlestickData>> GetCandlestickData(string symbol, DateTime startDate, DateTime endDate)
        {

            HttpResponseMessage response = await _httpClient.GetAsync($"engines/stock/markets/shares/securities/{symbol}/candles.json?from={startDate:yyyy-MM-dd}&till={endDate:yyyy-MM-dd}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Парсим JSON ответ
            var json = JsonConvert.DeserializeObject<JObject>(responseBody);
            var columns = json["candles"]["columns"].ToObject<List<string>>();
            var data = json["candles"]["data"].ToObject<List<List<object>>>();

            // Сопоставление данных с объектами CandlestickData
            List<CandlestickData> candlestickDataList = new List<CandlestickData>();
            foreach (var item in data)
            {
                var candlestickData = new CandlestickData
                {

                    Open = Convert.ToDecimal(item[columns.IndexOf("open")]),
                    Low = Convert.ToDecimal(item[columns.IndexOf("low")]),
                    High = Convert.ToDecimal(item[columns.IndexOf("high")]),
                    Close = Convert.ToDecimal(item[columns.IndexOf("close")]),
                    StartDate = DateTime.Parse(item[columns.IndexOf("begin")].ToString()),
                    EndDate = DateTime.Parse(item[columns.IndexOf("end")].ToString())
                };
                candlestickDataList.Add(candlestickData);
            }
            return candlestickDataList;
        }
    }
}

