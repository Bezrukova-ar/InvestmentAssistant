namespace InvestmentAssistant.Model.Strategy
{    /// <summary> Представляет структуру данных, используемую для сопоставления стратегии и ее условий </summary>
    class Strategy
    {
        /// <summary> Название стратегии</summary>
        public string Name { get; set; }
        /// <summary> Цель инвестирования</summary>
        public string InvestmentGoal { get; set; }
        /// <summary> Инвестиционный горизонт</summary>
        public string InvestmentHorizon { get; set; }
        /// <summary> Учёт рисков</summary>
        public string RiskAccounting { get; set; }
        /// <summary> Ожидаемая доходность</summary>
        public string ExpectedReturn { get; set; }
    }
}
