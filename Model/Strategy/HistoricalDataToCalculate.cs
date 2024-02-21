using System;

namespace InvestmentAssistant.Model.Strategy
{
    /// <summary> Представляет исторические данные для расчётов доходности и рисков</summary>
   public class HistoricalDataToCalculate
    {
        /// <summary> Уникальный код ценной бумаги </summary>
        public string SecurityId { get; set; }
        /// <summary> Режим торгов</summary>
        public string BoardID { get; set; }
        /// <summary> Представляет цену открытия </summary>
        public double Open { get; set; }
        /// <summary> Представляет цену закрытия  </summary>
        public double Close { get; set; }
        /// <summary> Представляет самую низкую цену </summary>
        public double Low { get; set; }
        /// <summary> Представляет самую высокую цену </summary>
        public double High { get; set; }
        /// <summary> Представляет дату торгов  </summary>
        public DateTime TradeDate { get; set; }
        /// <summary> Представляет расчитаную доходность </summary>
        public double Profitability{ get; set; }
        /// <summary> Представляет расчитанный риск </summary>
        public double Risk { get; set; }
    }
}
