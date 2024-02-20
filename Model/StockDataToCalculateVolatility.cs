using System;

namespace InvestmentAssistant.Model
{
    /// <summary> Представляет данные о ценах акций для расчета волатильности. </summary>
    public class StockDataToCalculateVolatility
    {
        /// <summary> Представляет режим торгов </summary>
        public string BoardID { get; set; }
        /// <summary> Представляет цену открытия </summary>
        public double Open { get; set; }
        /// <summary> Представляет самую низкую цену </summary>
        public double Low { get; set; }
        /// <summary> Представляет самую высокую цену </summary>
        public double High { get; set; }
        /// <summary> Представляет цену закрытия  </summary>
        public double Close { get; set; }
        /// <summary> Представляет дату торгов  </summary>
        public DateTime TradeDate { get; set; }
    }
}
