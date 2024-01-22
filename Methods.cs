using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace InvestmentAssistant
{
    
    class Methods
    {
        FinanceAPI financeAPI = new FinanceAPI();
        public Dictionary<string, NameOfSecurities> securitiesDictionary;

        /// <summary>
        /// Метод для заполнения данными о ценных бумагах хеш-таблицы
        /// </summary>
        public async Task LoadedData()
        {
            try
            {
                // Вызов метода GetListOfSecurities
                List<NameOfSecurities> securities = await financeAPI.GetListOfSecurities();
                // Инициализация хеш-таблицы
                securitiesDictionary = new Dictionary<string, NameOfSecurities>();
                // Заполнение хеш-таблицы данными, пропуская повторяющиеся значения
                foreach (var security in securities)
                {
                    if (!securitiesDictionary.ContainsKey(security.SecurityId))
                    {
                        securitiesDictionary.Add(security.SecurityId, security);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
