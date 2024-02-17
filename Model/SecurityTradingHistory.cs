using System;

namespace InvestmentAssistant.Model
{
    /// <summary> Представляет структуру данных, используемую для хранения информации об истории торгов ценной бумаги </summary>
    public class SecurityTradingHistory
    {
        /// <summary> Представляет режим торгов </summary>
        public string BoardID { get; set; }
        /// <summary> Представляет дату торгов </summary>
        public DateTime TradeDate { get; set; }
        /// <summary> Представляет число сделок </summary>
        public int NumTrade { get; set; }
        /// <summary> Представляет значение торгов </summary>
        public double Volume { get; set; }
    }
}
