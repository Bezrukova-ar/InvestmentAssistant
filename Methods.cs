using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace InvestmentAssistant
{
    
    class Methods
    {
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
    }
}
