using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentAssistant
{
    class StrategyAndConditions
    {   
        /// <summary> Перечень целей инвестирования</summary>
         public List<string> InvestmentGoalList = new List<string>
        {
            "Сохранение капитала",
            "Стабильный рост",
            "Максимизация доходности"
        };
        /// <summary> Перечень инвестиционных горизонтов</summary>
        public List<string> InvestmentHorizonList = new List<string>
        {
            "Краткосрочный",
            "Долгосрочный",
            "Среднесрочный"
        };
        /// <summary> Перечень рисков</summary>
        public List<string> RiskAccountingList = new List<string>
        {
            "Низкий",
            "Средний",
            "Высокий"
        };
        /// <summary> Перечень ожидаемой доходности</summary>
        public List<string> ExpectedReturnList = new List<string>
        {
            "Низкая",
            "Умеренная",
            "Средняя",
            "Высокая"
        };
    }
}
