using InvestmentAssistant.Model.Strategy;
using System.Collections.Generic;
using System.Windows.Controls;

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

        /// <summary> Метод выбора стратегии </summary>       
        public string DefinitionOfStrategy(ComboBox investmentGoalComboBox, ComboBox investmentHorizonComboBox, ComboBox riskAccountingComboBox, ComboBox expectedReturnComboBox, StrategyAndConditions strategyAndConditions, string[] userSelection)
        {
            string result;
            if (investmentGoalComboBox.SelectedItem == null || investmentHorizonComboBox.SelectedItem == null || riskAccountingComboBox.SelectedItem == null || expectedReturnComboBox.SelectedItem == null)
            {
                return result = "Заполните все поля";
            }
            int[,] matches = new int[strategyAndConditions.StrategyAndConditionsList.Count, userSelection.Length];

            for (int i = 0; i < strategyAndConditions.StrategyAndConditionsList.Count; i++)
            {
                Strategy strategy = strategyAndConditions.StrategyAndConditionsList[i];
                matches[i, 0] = strategy.InvestmentGoal == userSelection[0] ? 1 : 0;
                matches[i, 1] = strategy.InvestmentHorizon == userSelection[1] ? 1 : 0;
                matches[i, 2] = strategy.RiskAccounting == userSelection[2] ? 1 : 0;
                matches[i, 3] = strategy.ExpectedReturn == userSelection[3] ? 1 : 0;
            }

            int maxMatches = 0;
            int bestMatchIndex = -1;

            for (int i = 0; i < strategyAndConditions.StrategyAndConditionsList.Count; i++)
            {
                int currentMatches = 0;
                for (int j = 0; j < userSelection.Length; j++)
                {
                    currentMatches += matches[i, j];
                }

                if (currentMatches > maxMatches)
                {
                    maxMatches = currentMatches;
                    bestMatchIndex = i;
                }
            }

            if (bestMatchIndex != -1)
            {
                Strategy bestMatchedStrategy = strategyAndConditions.StrategyAndConditionsList[bestMatchIndex];
                result = bestMatchedStrategy.Name;
            }
            else
            {
                result = "Подходящая стратегия не найдена";
            }

            return result;
        }    
    }
}
