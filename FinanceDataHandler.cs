﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace InvestmentAssistant
{

    /// <summary>
    ///  Класс FinanceDataHandler предназначен для инкапсуляции и организации различных методов,
    ///  связанных с обработкой финансовых данных с использованием FinanceAPI
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
        /// Метод для заполнения данными для построения свечного графика хеш-таблицы
        /// </summary>
        public async Task FillCandlestickChartDataHash(string symbol, DateTime startDate, DateTime endDate, Hashtable candlestickChartDataHash)
        {
            /*var candlestickDataList = await financeAPI.GetCandlestickData(symbol, startDate, endDate);

             int index = 0;
             foreach (var candlestickData in candlestickDataList)
             {
                 candlestickChartDataHash.Add(index++, candlestickData);
             }*/
            var candlestickDataList = await financeAPI.GetCandlestickData(symbol, startDate, endDate);

            foreach (var candlestickData in candlestickDataList)
            {
                candlestickChartDataHash.Add(candlestickData.StartDate, candlestickData);
            }
        }
    }
}
