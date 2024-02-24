using InvestmentAssistant.Model;
using InvestmentAssistant.Model.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentAssistant
{

    /// <summary>
    /// Класс FinanceDataHandler предназначен для инкапсуляции методов, 
    /// связанных с обработкой финансовых данных через FinanceAPI
    /// </summary>
    class FinanceDataHandler
    {
        /// <summary> Экземпляр служит шлюзом для получения финансовых данных </summary>
        readonly FinanceAPI financeAPI = new FinanceAPI();

        /// <summary> Метод для заполнения данными о ценных бумагах словаря</summary>
        public async Task FillSecuritiesHashTable()
        {
            var listOfSecurities = await financeAPI.GetListOfSecurities();
            foreach (var security in listOfSecurities)
            {
                if (!MainWindow.securitiesHashTable.ContainsKey(security.SecurityId))
                {
                    MainWindow.securitiesHashTable.Add(security.SecurityId, security);
                }
            }
        }

        /// <summary> Метод для заполнения данными для построения свечного графика словаря</summary>
        public async Task FillCandlestickChartDictionary(string symbol, DateTime startDate, DateTime endDate, Dictionary<int, CandlestickData> candlestickChartDistionary)
        {
            var candlestickDataList = await financeAPI.GetCandlestickData(symbol, startDate, endDate);
            candlestickChartDistionary.Clear();
            int index = 1;
            foreach (var candlestickData in candlestickDataList)
            {
                candlestickChartDistionary.Add(index++, candlestickData);
            }
        }

        /// <summary> Метод для заполнения данными для построения графика объема сделок </summary>
        public async Task FillVolumeTradeDictionary(string symbol, DateTime startDate, DateTime endDate, Dictionary<int, SecurityTradingHistory> volumeTradeDictionary)
        {
            var securityTradingHistoryList = await financeAPI.GetTradingHistory(symbol, startDate, endDate);
            volumeTradeDictionary.Clear();
            int index = 1;
            foreach (var securityTradingHistory in securityTradingHistoryList)
            {
                volumeTradeDictionary.Add(index++, securityTradingHistory);
            }
        }

        /// <summary>Метод для заполнения данными для вывода наиболее возрасших и упавших акций</summary>
        public async Task FillThePriceChangeDictionary(Dictionary<int, SharePriceTodayAndYesterday> priceChangeDistionary)
        {
            var sharePriceTodayAndYesterdayList = await financeAPI.GetStockInfo();

            int index = 1;
            foreach (var stockInfo in sharePriceTodayAndYesterdayList)
            {
                if (stockInfo.CurrentValue != stockInfo.PreviousValue || stockInfo.PreviousValue != 0 || stockInfo.CurrentValue != 0)
                {
                    double percentageChange = (stockInfo.CurrentValue - stockInfo.PreviousValue) / stockInfo.PreviousValue * 100;
                    if (!double.IsInfinity(percentageChange))
                    {
                        priceChangeDistionary.Add(index++, new SharePriceTodayAndYesterday
                        {
                            SecurityId = stockInfo.SecurityId,
                            SecurityName = stockInfo.SecurityName,
                            BoardID = stockInfo.BoardID,
                            CurrentValue = stockInfo.CurrentValue,
                            PreviousValue = stockInfo.PreviousValue,
                            PercentageChangeInValue = percentageChange
                        });
                    }
                }
            }
        }

        /// <summary> Метод для заполнения данными для расчёта волатильности/// </summary>
        public async Task FillStockDataToCalculateVolatilityDictionary(string symbol, Dictionary<int, StockDataToCalculateVolatility> dataToCalculateVolatilityDictionary)
        {
            var stockDataToCalculateVolatilityList = await financeAPI.GetDataToCalculateVolatility(symbol);
            dataToCalculateVolatilityDictionary.Clear();
            int index = 1;
            foreach (var stockDataToCalculateVolatility in stockDataToCalculateVolatilityList)
            {
                dataToCalculateVolatilityDictionary.Add(index++, stockDataToCalculateVolatility);
            }
        }
        /// <summary> Метод для заполнения данными о ценных бумагах для формирования портфеля</summary>
        public async Task FillStockDataList(List<StockData> stockDataList)
        {
            var stockData = await financeAPI.GetListOfSecuritiesTakingIntoAccountTheTradingMode();
            foreach (var security in stockData)
            {
                if(security.CurrentSharePrice!=0)
                {
                    stockDataList.Add(new StockData
                    {
                        SecurityId = security.SecurityId,
                        SecurityName = security.SecurityName,
                        BoardID = security.BoardID,
                        CurrentSharePrice = security.CurrentSharePrice
                    });
                }               
            }
        }

        /// <summary> Метод для заполнения данными для расчёта</summary>
        public async Task FillDataForCalculationsList(List<HistoricalDataToCalculate> dataForCalculationsList, List<StockData> stockDataList, IProgress<int> progress)
        {
            var uniqueSecIDs = stockDataList.Select(s => s.SecurityId).Distinct().ToList();

            // Общее количество элементов для обработки
            int totalItems = uniqueSecIDs.Count;
            int processedItems = 0;

            foreach (var symbol in uniqueSecIDs)
            {
                var dataForCalculations = await financeAPI.GetListToCalculateProfitability(symbol);

                foreach (var data in dataForCalculations)
                {

                    dataForCalculationsList.Add(new HistoricalDataToCalculate
                    {
                        SecurityId = data.SecurityId,
                        BoardID = data.BoardID,
                        Open = data.Open,
                        Close = data.Close,
                        TradeDate = data.TradeDate,
                        High = data.High,
                        Low = data.Low,
                        Profitability = (data.Close - data.Open) / data.Open * 100,
                        Risk = (data.High - data.Low) / ((data.High + data.Low) / 2)
                    });

                }
                // Увеличиваем количество обработанных элементов
                processedItems++;

                // Вычисляем прогресс в процентах
                int progressPercentage = processedItems * 100 / totalItems;

                // Отправляем прогресс через объект Progress
                progress.Report(progressPercentage);
            }
        }       
    }
}
