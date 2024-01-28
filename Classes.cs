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
    public class StockParametersForChart
    {
        public string Symbol { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string HistoricalData { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CandlestickData
    {
      
        public decimal Open { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Close { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
