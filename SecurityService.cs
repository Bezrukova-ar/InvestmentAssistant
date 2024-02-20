using InvestmentAssistant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace InvestmentAssistant
{
    /// <summary> Класс SecurityService предназначен для работы с данными по ценным бумагам</summary>
    class SecurityService
    {
        /// <summary>
        /// Метод поиска кода акции по названию
        /// </summary>
        public string GetIdSecurityByName(string nameSecurity)
        {
            foreach (var entry in MainWindow.securitiesHashTable.Values)
            {
                if (entry is NameOfSecurities security && security.SecurityName == nameSecurity)
                {
                    return security.SecurityId;
                }
            }
            MessageBox.Show("Не найден код для введенной акции, выберите другую или допишите название"); // Если запись не найдена

            return null;
        }

        /// <summary>
        /// Метод выборки 3х наиболее возросших акций по каждому виду торгов
        /// </summary>
        public Dictionary<string, List<SharePriceTodayAndYesterday>> GetTopGainers(Dictionary<int, SharePriceTodayAndYesterday> priceChangeDictionary)
        {
            Dictionary<string, List<SharePriceTodayAndYesterday>> topGainers = new Dictionary<string, List<SharePriceTodayAndYesterday>>();
            var groups = priceChangeDictionary.Values.GroupBy(x => x.BoardID);
            foreach (var group in groups)
            {
                List<SharePriceTodayAndYesterday> sortedList = group.OrderByDescending(x => x.PercentageChangeInValue)
                    .Take(3)
                    .ToList();
                topGainers.Add(group.Key, sortedList);
            }

            return topGainers;
        }

        /// <summary>
        /// Метод выборки 3х наиболее упавших акций по каждому виду торгов
        /// </summary>
        public Dictionary<string, List<SharePriceTodayAndYesterday>> GetWorseLosers(Dictionary<int, SharePriceTodayAndYesterday> priceChangeDictionary)
        {
            Dictionary<string, List<SharePriceTodayAndYesterday>> topLosers = new Dictionary<string, List<SharePriceTodayAndYesterday>>();
            var groups = priceChangeDictionary.Values.GroupBy(x => x.BoardID);
            foreach (var group in groups)
            {
                List<SharePriceTodayAndYesterday> sortedList = group.OrderBy(x => x.PercentageChangeInValue).Take(3).ToList();
                topLosers.Add(group.Key, sortedList);
            }

            return topLosers;
        }

        /// <summary>
        /// Метод расчета волатильности (стандартное отклонение)
        /// </summary>
        public string CalculationOfStandardDeviation(Dictionary<int, StockDataToCalculateVolatility> dataToCalculateVolatilityDictionary)
        {
            string result = "";
            foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
            {
                string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                double sumSquaredDifferences = 0;
                double averageClose = group.Average(d => d.Value.Close);
                int numOfDays = group.Count();

                foreach (var data in group)
                {
                    sumSquaredDifferences += Math.Pow(data.Value.Close - averageClose, 2);
                }

                double volatility = Math.Sqrt(sumSquaredDifferences / numOfDays);
                result += $"Стандартное отклонение для режима торгов {boardID}: { Math.Round(volatility, 5)}\n";
            }

            return result;
        }

        /// <summary>
        /// Метод расчета волатильности (среднее истинное назначение)
        /// </summary>
        public string AverageTrueRangeCalculation(Dictionary<int, StockDataToCalculateVolatility> dataToCalculateVolatilityDictionary)
        {
            string result = "";
            foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
            {
                string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                double sumATR = 0;
                int numOfDays = group.Count();
                double previousClose = group.Select(x => x.Value.Close).FirstOrDefault();
                foreach (var data in group)
                {
                    double highLowDifference = data.Value.High - data.Value.Low;
                    double highPreviousCloseDifference = Math.Abs(data.Value.High - previousClose);
                    double lowPreviousCloseDifference = Math.Abs(data.Value.Low - previousClose);
                    double currentATR = Math.Max(highLowDifference, Math.Max(highPreviousCloseDifference, lowPreviousCloseDifference));
                    sumATR += currentATR;
                    previousClose = data.Value.Close;
                }
                double ATR = sumATR / numOfDays;
                result += $"Средний истинный диапазон для режима торгов {boardID}: { Math.Round(ATR, 5)}\n";
            }

            return result;
        }

        /// <summary>
        /// Метод расчета волатильности (индекс волатильности)
        /// </summary>
        public string VolatilityIndexCalculation(Dictionary<int, StockDataToCalculateVolatility> dataToCalculateVolatilityDictionary)
        {
            string result = "";
            foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
            {
                string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                double sumPercentageDifference = 0;
                int numOfDays = group.Count();

                foreach (var data in group)
                {
                    double high = data.Value.High;
                    double low = data.Value.Low;

                    sumPercentageDifference += (high - low) / high;
                }
                double volatility = (sumPercentageDifference / numOfDays) * 100;
                result += $"Индекс волатильности для режима торгов {boardID}: { Math.Round(volatility, 5)}\n";
            }

            return result;
        }

        /// <summary>
        /// Метод расчета волатильности (среднего отклонения)
        /// </summary>
        public string CalculationOfAverageDeviation(Dictionary<int, StockDataToCalculateVolatility> dataToCalculateVolatilityDictionary)
        {
            string result = "";
            foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
            {
                string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                double sumAbsoluteDifferences = 0;
                double averageClose = group.Average(d => d.Value.Close);
                int numOfDays = group.Count();
                foreach (var data in group)
                {
                    sumAbsoluteDifferences += Math.Abs(data.Value.Close - averageClose);
                }
                double volatility = sumAbsoluteDifferences / numOfDays;
                result += $"Среднее отклонение для режима торгов {boardID}: { Math.Round(volatility, 5)}\n";
            }

            return result;
        }
    }
}
