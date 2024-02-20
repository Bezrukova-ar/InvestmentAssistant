using System;

namespace InvestmentAssistant.Model
{
    /// <summary>Класс, представляющий информацию о цене акции (свежая цена и прошлая цена)</summary>
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
}
