namespace InvestmentAssistant.Model.Strategy
{
    /// <summary> Представляет данные о ценных бумагах</summary>
    public class StockData
    {
        /// <summary> Уникальный код ценной бумаги </summary>
        public string SecurityId { get; set; }
        /// <summary> Название ценной бумаги </summary>
        public string SecurityName { get; set; }
        /// <summary> Режим торгов</summary>
        public string BoardID { get; set; }
        /// <summary> Актуальная цена акции</summary>
        public double CurrentSharePrice { get; set; }
        /// <summary> Прогнозируемая доходность акции </summary>
        public double ProjectedStockReturn { get; set; }
        /// <summary> Риск акции </summary>
        public double StockRisk { get; set; }
    }
}
