namespace InvestmentAssistant.Model.Strategy
{
    /// <summary> Представляет данные о ценных бумагах</summary>
    class StockData
    {
        /// <summary> Уникальный код ценной бумаги </summary>
        public string SecurityId { get; set; }
        /// <summary> Название ценной бумаги </summary>
        public string SecurityName { get; set; }
        /// <summary> Прогнозируемая доходность акции </summary>
        public string ProjectedStockReturn { get; set; }
        /// <summary> Риск акции </summary>
        public string StockRisk { get; set; }
    }
}
