namespace InvestmentAssistant.Model
{
    /// <summary> Представляет экземпляр ценной бумаги с уникальным кодом и названием </summary>
    public class NameOfSecurities
    {
        /// <summary> Уникальный код ценной бумаги </summary>
        public string SecurityId { get; set; }
        /// <summary> Название ценной бумаги </summary>
        public string SecurityName { get; set; }
    }
}
