using System;

namespace InvestmentAssistant
{
    /// <summary> Представляет экземпляр ценной бумаги с уникальным кодом и названием </summary>
    public class NameOfSecurities
    {  
        /// <summary> Уникальный код ценной бумаги </summary>
        public string SecurityId { get; set; }
        /// <summary> Название ценной бумаги </summary>
        public string SecurityName { get; set; }
    }

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

    public class SharePriceTodayAndYesterday
    {
        /// <summary> Уникальный код ценной бумаги </summary>
        public string SecurityId { get; set; }
        /// <summary> Название ценной бумаги </summary>
        public string SecurityName { get; set; }
        /// <summary> Представляет режим торгов </summary>
        public string BoardID { get; set; }
        /// <summary> Актуальная на сегодня стоимость бумаги </summary>
        public double CurrentValue { get; set; }
        /// <summary> Прошлая стоимость бумаги </summary>
        public double PreviousValue { get; set; }
        /// <summary> Изменение бумаги в % </summary>
        public double PercentageChangeInValue { get; set; }

        public static explicit operator SharePriceTodayAndYesterday(double v)
        {
            throw new NotImplementedException();
        }
    }

    // Создание модели данных для графика (не используется там где апи суванье данных)
    public class GainerData
    {
        public string SecurityName { get; set; }
        public double PercentageChange { get; set; }
        public int Index { get; set; }
    }


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
