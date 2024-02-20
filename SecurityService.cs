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
    }
}
