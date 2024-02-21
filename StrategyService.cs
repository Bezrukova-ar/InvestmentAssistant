using InvestmentAssistant.Model;
using InvestmentAssistant.Model.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InvestmentAssistant
{
    class StrategyService
    { 
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
