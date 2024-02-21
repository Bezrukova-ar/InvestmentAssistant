using InvestmentAssistant.Model;
using InvestmentAssistant.Model.Strategy;
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

        /// <summary> Сравнительная таблица стратегия и условия когда она применяется</summary>
        public List<Strategy> StrategyAndConditionsList = new List<Strategy>
        {
            new Strategy
            {
                Name = "Консервативная",
                InvestmentGoal = "Сохранение капитала",
                InvestmentHorizon = "Краткосрочный",
                RiskAccounting = "Низкий",
                ExpectedReturn = "Низкая"
            },
            new Strategy
            {
                Name = "Сбалансированная",
                InvestmentGoal = "Стабильный рост",
                InvestmentHorizon = "Среднесрочный",
                RiskAccounting = "Средний",
                ExpectedReturn = "Умеренная"
            },
            new Strategy
            {
                Name = "Агрессивная",
                InvestmentGoal = "Максимизация доходности",
                InvestmentHorizon = "Долгосрочный",
                RiskAccounting = "Высокий",
                ExpectedReturn = "Высокая"
            },
            new Strategy
            {
                Name = "Пассивная",
                InvestmentGoal = "Сохранение капитала",
                InvestmentHorizon = "Долгосрочный",
                RiskAccounting = "Низкий",
                ExpectedReturn = "Средняя"
            },
            new Strategy
            {
                Name = "Активная",
                InvestmentGoal = "Максимизация доходности",
                InvestmentHorizon = "Краткосрочный",
                RiskAccounting = "Высокий",
                ExpectedReturn = "Высокая"
            },
        };
    }
}
