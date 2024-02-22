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

        /// <summary> Прогнозирование доходности акции методом экспоненциального сглаживания</summary>
        public void ExponentialSmoothingProfitability(List<StockData> stockDataList, double[] data, double alpha, string securityId, string boardId)
        {
            double forecast = 0;

            // Инициализируем прогноз первым значением
            forecast = data[0];

            // Применяем экспоненциальное сглаживание для остальных значений
            for (int i = 0; i < data.Length; i++)
            {
                forecast = alpha * data[i] + (1 - alpha) * forecast;
                // Поиск индекса элемента в stockDataList с совпадающим SecurityId и BoardID                
                int index = stockDataList.FindIndex(x => x.SecurityId == securityId && x.BoardID == boardId);
                if (index != -1)
                    stockDataList[index].ProjectedStockReturn = forecast;
                else /*что-то додумать, добавлять новые записи, а еще обрабатывать случаи когда рассчитывается не число*/;
            }            
        }

        /// <summary> Прогнозирование рисков акции методом экспоненциального сглаживания</summary>
        public void ExponentialSmoothingRisk(List<StockData> stockDataList, double[] data, double alpha, string securityId, string boardId)
        {
            double forecast = 0;

            // Инициализируем прогноз первым значением
            forecast = data[0];

            // Применяем экспоненциальное сглаживание для остальных значений
            for (int i = 1; i < data.Length; i++)
            {
                forecast = alpha * data[i] + (1 - alpha) * forecast;
                // Поиск индекса элемента в stockDataList с совпадающим SecurityId и BoardID
                int index = stockDataList.FindIndex(x => x.SecurityId == securityId && x.BoardID == boardId);
                if (index != -1)
                    stockDataList[index].StockRisk = forecast;
                else /*что-то додумать, добавлять новые записи, а еще обрабатывать случаи когда рассчитывается не число*/;
            }         
        }

        /// <summary> Вычисление оптимального значения коэффициента сглаживания. Методом наименьших квадратов</summary>
        public double AutoExponentialSmoothing(double[] data)
        {
            double bestAlpha = 0.0;
            double bestMSE = double.MaxValue;

            for (double alpha = 0.1; alpha < 1.0; alpha += 0.1)
            {
                double forecast = 0;
                double mse = 0;

                forecast = data[0];

                for (int i = 1; i < data.Length; i++)
                {
                    forecast = alpha * data[i] + (1 - alpha) * forecast;
                    mse += Math.Pow(data[i] - forecast, 2);
                }

                mse /= data.Length;

                if (mse < bestMSE)
                {
                    bestMSE = mse;
                    bestAlpha = alpha;
                }
            }

            return bestAlpha;
        }

        /// <summary>Подбор акций для формирования инвестиционного портфеля методом консервативной стратегии</summary>
        public List<Portfolio> OptimizePortfolio(List<StockData> stockDataList, double maxCapital)
        {
            var portfolio = new List<Portfolio>();
            var remainingCapital = maxCapital;

            while (remainingCapital > 0)
            {
                StockData selectedStock = null;
                double maxExpectedReturn = 0;

                foreach (var stock in stockDataList)
                {
                    if (stock.ProjectedStockReturn >= maxExpectedReturn && stock.CurrentSharePrice <= remainingCapital)
                    {
                        selectedStock = stock;
                        maxExpectedReturn = stock.ProjectedStockReturn;
                    }
                }

                if (selectedStock != null)
                {
                    var shareCount = Math.Floor(remainingCapital / selectedStock.CurrentSharePrice);
                    var portfolioValue = shareCount * selectedStock.CurrentSharePrice;

                    if (portfolioValue > remainingCapital)
                    {
                        shareCount--;
                    }

                    if (shareCount > 0)
                    {
                        portfolio.Add(new Portfolio
                        {
                            SecurityId = selectedStock.SecurityId,
                            name = selectedStock.SecurityName,
                            cena= selectedStock.CurrentSharePrice,
                            ShareCount = shareCount
                        });

                        remainingCapital -= shareCount * selectedStock.CurrentSharePrice;
                    }
                }
                else
                {
                    break;
                }
            }

            return portfolio;
        }

        public class Portfolio
        {
            public string SecurityId { get; set; }
            public string name { get; set; }
            public double cena { get; set; }
            public double ShareCount { get; set; }
        }
    }
}
