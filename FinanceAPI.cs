using InvestmentAssistant.Model;
using InvestmentAssistant.Model.Strategy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvestmentAssistant
{

    /// <summary> Класс FinanceAPI инкапсулирует функциональные возможности взаимодействия с API Московской биржи(MOEX) 
    /// для получения информации о ценных бумагах </summary>
    public class FinanceAPI
    {
        private const string FinanceApiUrl = "https://iss.moex.com/iss/";
        private readonly HttpClient _httpClient;

        public FinanceAPI()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(FinanceApiUrl)
            };
        }

        /// <summary> Метод получения списка бумаг на московской бирже </summary>
        public async Task<List<NameOfSecurities>> GetListOfSecurities()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"engines/stock/markets/shares/securities.json");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Парсим JSON ответ
            var json = JsonConvert.DeserializeObject<JObject>(responseBody);
            var columns = json["securities"]["columns"].ToObject<List<string>>();
            var data = json["securities"]["data"].ToObject<List<List<object>>>();

            // Сопоставление данных с объектами CandlestickData
            List<NameOfSecurities> nameOfSecuritiesList = new List<NameOfSecurities>();
            foreach (var item in data)
            {
                var nameOfSecurities = new NameOfSecurities
                {
                    SecurityId=Convert.ToString(item[columns.IndexOf("SECID")]),
                    SecurityName = Convert.ToString(item[columns.IndexOf("SECNAME")])
                };
                nameOfSecuritiesList.Add(nameOfSecurities);
            }

            return nameOfSecuritiesList;
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

                    Open = Convert.ToDouble(item[columns.IndexOf("open")]),
                    Low = Convert.ToDouble(item[columns.IndexOf("low")]),
                    High = Convert.ToDouble(item[columns.IndexOf("high")]),
                    Close = Convert.ToDouble(item[columns.IndexOf("close")]),
                    StartDate = DateTime.Parse(item[columns.IndexOf("begin")].ToString()),
                    EndDate = DateTime.Parse(item[columns.IndexOf("end")].ToString())
                };
                candlestickDataList.Add(candlestickData);
            }

            return candlestickDataList;
        }

        /// <summary> Метод получения данных истории торгов для построения диаграммы объемов торгов </summary>
        public async Task<List<SecurityTradingHistory>> GetTradingHistory(string symbol, DateTime startDate, DateTime endDate)
        {
            string requestUri = $"history/engines/stock/markets/shares/securities/{symbol}/trades.json?from={startDate:yyyy-MM-dd}&till={endDate:yyyy-MM-dd}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Парсим JSON ответ
            var json = JsonConvert.DeserializeObject<JObject>(responseBody);
            var columns = json["history"]["columns"].ToObject<List<string>>();
            var data = json["history"]["data"].ToObject<List<List<object>>>();

            // Сопоставление данных с объектами securityTradingHistoryList
            List<SecurityTradingHistory> securityTradingHistoryList = new List<SecurityTradingHistory>();
            foreach (var item in data)
            {
                var securityTradingHistory = new SecurityTradingHistory
                {
                    BoardID = Convert.ToString(item[columns.IndexOf("BOARDID")]),
                    TradeDate = DateTime.Parse(item[columns.IndexOf("TRADEDATE")].ToString()),
                    NumTrade = Convert.ToInt32(item[columns.IndexOf("NUMTRADES")]),
                    Volume = Convert.ToDouble(item[columns.IndexOf("VOLUME")])
                };
                securityTradingHistoryList.Add(securityTradingHistory);
            }

            return securityTradingHistoryList;
        }

        /// <summary> Метод получения данных для расчета изменения цены акции в процентах</summary>
        public async Task<List<SharePriceTodayAndYesterday>> GetStockInfo()
        {
            string requestUri = $"engines/stock/markets/shares/securities.json?";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Парсим JSON ответ
            var json = JsonConvert.DeserializeObject<JObject>(responseBody);
            var columns = json["securities"]["columns"].ToObject<List<string>>();
            var data = json["securities"]["data"].ToObject<List<List<object>>>();

            // Сопоставление данных с объектами securityTradingHistoryList
            List<SharePriceTodayAndYesterday> sharePriceTodayAndYesterdayList = new List<SharePriceTodayAndYesterday>();
            foreach (var item in data)
            {
                var sharePriceTodayAndYesterday = new SharePriceTodayAndYesterday
                {
                    SecurityId = Convert.ToString(item[columns.IndexOf("SECID")]),
                    BoardID = Convert.ToString(item[columns.IndexOf("BOARDID")]),
                    SecurityName = Convert.ToString(item[columns.IndexOf("SECNAME")].ToString()),
                    CurrentValue = Convert.ToDouble(item[columns.IndexOf("PREVPRICE")]),
                    PreviousValue = Convert.ToDouble(item[columns.IndexOf("PREVWAPRICE")])
                };
                sharePriceTodayAndYesterdayList.Add(sharePriceTodayAndYesterday);
            }

            return sharePriceTodayAndYesterdayList;
        }

        /// <summary> Метод получения данных для расчета волатильности</summary>
        public async Task<List<StockDataToCalculateVolatility>> GetDataToCalculateVolatility(string symbol)
        {
            string fromDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            string toDate = DateTime.Now.ToString("yyyy-MM-dd");

            string requestUri = $"history/engines/stock/markets/shares/securities/{symbol}.json?from={fromDate}&till={toDate}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Парсим JSON ответ
            var json = JsonConvert.DeserializeObject<JObject>(responseBody);
            var columns = json["history"]["columns"].ToObject<List<string>>();
            var data = json["history"]["data"].ToObject<List<List<object>>>();

            // Сопоставление данных с объектами StockDataToCalculateVolatility
            List<StockDataToCalculateVolatility> StockDataToCalculateVolatilityList = new List<StockDataToCalculateVolatility>();
            foreach (var item in data)
            {
                var stockDataToCalculateVolatility = new StockDataToCalculateVolatility
                {
                    BoardID = Convert.ToString(item[columns.IndexOf("BOARDID")]),
                    Open = Convert.ToDouble(item[columns.IndexOf("OPEN")]),
                    Low = Convert.ToDouble(item[columns.IndexOf("LOW")]),
                    High = Convert.ToDouble(item[columns.IndexOf("HIGH")]),
                    Close = Convert.ToDouble(item[columns.IndexOf("CLOSE")]),
                    TradeDate = DateTime.Parse(item[columns.IndexOf("TRADEDATE")].ToString()),
                };
                StockDataToCalculateVolatilityList.Add(stockDataToCalculateVolatility);
            }

            return StockDataToCalculateVolatilityList;
        }

        /// <summary> Метод получения списка бумаг на московской бирже с учётом режима торгов </summary>
        public async Task<List<StockData>> GetListOfSecuritiesTakingIntoAccountTheTradingMode()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"engines/stock/markets/shares/securities.json");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Парсим JSON ответ
            var json = JsonConvert.DeserializeObject<JObject>(responseBody);
            var columns = json["securities"]["columns"].ToObject<List<string>>();
            var data = json["securities"]["data"].ToObject<List<List<object>>>();

            // Сопоставление данных с объектами CandlestickData
            List<StockData> StockDataList = new List<StockData>();
            foreach (var item in data)
            {
                var stockData = new StockData
                {
                    SecurityId = Convert.ToString(item[columns.IndexOf("SECID")]),
                    SecurityName = Convert.ToString(item[columns.IndexOf("SECNAME")]),
                    BoardID = Convert.ToString(item[columns.IndexOf("BOARDID")]),
                    CurrentSharePrice=Convert.ToDouble(item[columns.IndexOf("PREVPRICE")])
                };
                StockDataList.Add(stockData);
            }

            return StockDataList;
        }

        /// <summary> Метод получения исторических данных для расчета доходности </summary>
        public async Task<List<HistoricalDataToCalculate>> GetListToCalculateProfitability(string symbol)
        {
            string fromDate = DateTime.Now.AddYears(-3).ToString("yyyy-MM-dd");
            string toDate = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await _httpClient.GetAsync($"history/engines/stock/markets/shares/securities/{symbol}.json?from={fromDate}&till={toDate}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Парсим JSON ответ
            var json = JsonConvert.DeserializeObject<JObject>(responseBody);
            var columns = json["history"]["columns"].ToObject<List<string>>();
            var data = json["history"]["data"].ToObject<List<List<object>>>();

            // Сопоставление данных с объектами CandlestickData
            List<HistoricalDataToCalculate> HistoricalDataToCalculateList = new List<HistoricalDataToCalculate>();
            foreach (var item in data)
            {
                var historicalDataToCalculateList = new HistoricalDataToCalculate
                {
                    SecurityId = Convert.ToString(item[columns.IndexOf("SECID")]),
                    BoardID = Convert.ToString(item[columns.IndexOf("BOARDID")]),
                    Open = Convert.ToDouble(item[columns.IndexOf("OPEN")]),
                    Close = Convert.ToDouble(item[columns.IndexOf("CLOSE")]),
                    TradeDate = Convert.ToDateTime(item[columns.IndexOf("TRADEDATE")]),
                    High = Convert.ToDouble(item[columns.IndexOf("HIGH")]),
                    Low = Convert.ToDouble(item[columns.IndexOf("LOW")])
                };
                HistoricalDataToCalculateList.Add(historicalDataToCalculateList);
            }

            return HistoricalDataToCalculateList;
        }
    }
}

