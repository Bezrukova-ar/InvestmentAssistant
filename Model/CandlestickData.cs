using System;

namespace InvestmentAssistant.Model
{
    /// <summary> Представляет структуру данных, используемую для хранения информации о финансовых свечах </summary>
    public class CandlestickData
    {
        /// <summary> Представляет цену открытия </summary>
        public double Open { get; set; }
        /// <summary> Представляет самую низкую цену </summary>
        public double Low { get; set; }
        /// <summary> Представляет самую высокую цену </summary>
        public double High { get; set; }
        /// <summary> Представляет цену закрытия </summary>
        public double Close { get; set; }
        /// <summary> Представляет начальную дату и время периода времени, к которому относятся данные свечи </summary>
        public DateTime StartDate { get; set; }
        /// <summary> Представляет конечную дату и время периода времени, к которому относятся данные свечи </summary>
        public DateTime EndDate { get; set; }
    }
}