using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace InvestmentAssistant
{
    
    class Methods
    {
        FinanceAPI financeAPI = new FinanceAPI();
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
    }
}
