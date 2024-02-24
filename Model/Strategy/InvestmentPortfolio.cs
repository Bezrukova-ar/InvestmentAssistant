namespace InvestmentAssistant.Model.Strategy
{
    /// <summary> Структура данных инвестиционного портфеля</summary>
    public class InvestmentPortfolio
    {
        /// <summary> Уникальный код ценной бумаги </summary>
        public string SecurityId { get; set; }
        /// <summary> Название ценной бумаги </summary>
        public string SecurityName { get; set; }
        /// <summary> Режим торгов</summary>
        public string BoardID { get; set; }
        /// <summary> Количество бумаг</summary>
        public double Quantity { get; set; }
        /// <summary> Затраченная сумма денег (цена*количество)</summary>
        public double TotalInvestment { get; set; }
    }
}
