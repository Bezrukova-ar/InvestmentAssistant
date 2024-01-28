using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentAssistant
{
    /// <summary> Представляет экземпляр ценной бумаги с уникальным кодом и названием </summary>
    public class NameOfSecurities
    {  /// <summary> Уникальный код ценной бумаги </summary>
        public string SecurityId { get; set; }
        /// <summary> Название ценной бумаги </summary>
        public string SecurityName { get; set; }
    }
    /* public class StockParametersForChart
     {
         public string Symbol { get; set; }
         public DateTime StartDate { get; set; }
         public DateTime EndDate { get; set; }
         public string HistoricalData { get; set; }
         public string ErrorMessage { get; set; }
     }*/

    /// <summary> Представляет структуру данных, используемую для хранения информации о финансовых свечах </summary>
    public class CandlestickData
    {
        /// <summary> Представляет цену открытия </summary>
        public decimal Open { get; set; }
        /// <summary> Представляет самую низкую цену </summary>
        public decimal Low { get; set; }
        /// <summary> Представляет самую высокую цену </summary>
        public decimal High { get; set; }
        /// <summary> Представляет цену закрытия  </summary>
        public decimal Close { get; set; }
        /// <summary> Представляет начальную дату и время периода времени, к которому относятся данные свечи </summary>
        public DateTime StartDate { get; set; }
        /// <summary> Представляет конечную дату и время периода времени, к которому относятся данные свечи </summary>
        public DateTime EndDate { get; set; }
    }
}
