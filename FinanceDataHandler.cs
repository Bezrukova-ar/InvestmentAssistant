using InvestmentAssistant.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

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

        /// <summary>
        /// Метод для заполнения данными о ценных бумагах хеш-таблицы
        /// </summary>
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

        /// <summary>
        /// Метод для заполнения данными для построения свечного графика хеш-таблицы
        /// </summary>
        public async Task FillCandlestickChartDataHash(string symbol, DateTime startDate, DateTime endDate, Hashtable candlestickChartDataHash)
        {
            var candlestickDataList = await financeAPI.GetCandlestickData(symbol, startDate, endDate);

            int index = 1;
            foreach (var candlestickData in candlestickDataList)
            {
                candlestickChartDataHash.Add(index++, candlestickData);
            }
        }

        /// <summary> Метод для заполнения данными для построения графика объема сделок </summary>
        public async Task FillVolumeTradeDataHash(string symbol, DateTime startDate, DateTime endDate, Hashtable volumeTradeDataHash)
        {
            var securityTradingHistoryList = await financeAPI.GetTradingHistory(symbol, startDate, endDate);

            int index = 1;
            foreach (var securityTradingHistory in securityTradingHistoryList)
            {
                volumeTradeDataHash.Add(index++, securityTradingHistory);
            }
        }

        /// <summary>Метод для заполнения данными для вывода наиболее возрасших и упавших акций</summary>
        public async Task FillThePriceChangeHashTable(Hashtable priceChangeHashTable)
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
                        priceChangeHashTable.Add(index++, new SharePriceTodayAndYesterday
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

        /// <summary>
        /// Метод для заполнения данными для расчёта волатильности
        /// </summary>
        public async Task FillStockDataToCalculateVolatility(string symbol, Dictionary<int, StockDataToCalculateVolatility> dataToCalculateVolatility)
        {
            var stockDataToCalculateVolatilityList = await financeAPI.GetDataToCalculateVolatility(symbol);

            int index = 1;
            foreach (var stockDataToCalculateVolatility in stockDataToCalculateVolatilityList)
            {
                dataToCalculateVolatility.Add(index++, stockDataToCalculateVolatility);
            }
        }
    }
}
