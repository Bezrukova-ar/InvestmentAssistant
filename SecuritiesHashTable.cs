using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentAssistant
{
    /// <summary>
    /// Класс управляет Хеш таблицей и предоставляет  методы для работы с ней
    /// </summary>
    public class SecuritiesHashTable
    {
        private readonly Dictionary<string, (string, string, string)> _securities;

        /// <summary>
        /// Конструктор класса SecuritiesHashTable 
        /// Код инициализирует новый экземпляр словаря для хранения информации о ценных бумагах, 
        /// используя строки в качестве ключей и кортежи строк в качестве значений
        /// </summary>
        public SecuritiesHashTable()
        {
            _securities = new Dictionary<string, (string, string, string)>();
        }
        /// <summary>
        /// Метод класса, который добавляет новые ценные бумаги в словарь
        /// </summary>
        public void AddSecurities(List<(string, string, string)> securities)
        {
            foreach (var security in securities)
            {
                _securities[security.Item1] = security;
            }
        }
        /// <summary>
        /// Метод класса, который возвращает все ценные бумаги
        /// </summary>
        public List<(string, string, string)> GetAllSecurities()
        {
            return _securities.Values.ToList();
        }

        /// <summary>
        /// Метод класса, который возвращает информацию о ценной бумаге по ее идентификатору
        /// </summary>
        //public (string, string, string) GetSecurityById(string secId)
        //{
        //    if (_securities.TryGetValue(secId, out var security))
        //    {
        //        return security;
        //    }
        //    else
        //    {
        //        return (string.Empty, string.Empty, string.Empty);
        //    }
        //}

    }
}
