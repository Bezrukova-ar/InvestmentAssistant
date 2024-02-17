using InvestmentAssistant.Model;
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
    }
}
