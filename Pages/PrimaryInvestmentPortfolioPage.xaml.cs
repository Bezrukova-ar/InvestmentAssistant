using InvestmentAssistant.Model.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace InvestmentAssistant.Pages
{
    public partial class PrimaryInvestmentPortfolioPage : Page
    {
        /// <summary> Экземпляр класса для заполнения данными combobox</summary>
        StrategyAndConditions strategyAndConditions = new StrategyAndConditions();
        /// <summary> Экземпляр класса для стратегий</summary>
        StrategyService strategyService = new StrategyService();
        /// <summary> Экземпляр класса для заполнения таблиц финансовыми данными </summary>
        FinanceDataHandler financeDataHandler = new FinanceDataHandler();

        /// <summary> Коллекция, где хранится информация для формирования инвестиционного портфеля</summary>
        List<StockData> stockDataList = new List<StockData>();
        /// <summary> Коллекция, где хранится информация для расчетов для формирования инвестиционного портфеля</summary>
        List<HistoricalDataToCalculate> dataForCalculationsList = new List<HistoricalDataToCalculate>();
        /// <summary> Коллекция, где хранится список инвестиционного портфеля</summary>
        List<InvestmentPortfolio> portfolios = new List<InvestmentPortfolio>();

        string[] userSelection = { null, null, null, null };
        double capital;

        public PrimaryInvestmentPortfolioPage()
        {
            InitializeComponent();
            investmentGoalComboBox.ItemsSource = strategyAndConditions.InvestmentGoalList;
            investmentHorizonComboBox.ItemsSource = strategyAndConditions.InvestmentHorizonList;
            riskAccountingComboBox.ItemsSource = strategyAndConditions.RiskAccountingList;
            expectedReturnComboBox.ItemsSource = strategyAndConditions.ExpectedReturnList;
        }

        /// <summary> Ввод только тех значений, что являются числом </summary>
        private void capitalTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void investmentGoalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[0] = investmentGoalComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = strategyService.DefinitionOfStrategy(investmentGoalComboBox, investmentHorizonComboBox, riskAccountingComboBox, expectedReturnComboBox, strategyAndConditions, userSelection);
        }

        private void investmentHorizonComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[1] = investmentHorizonComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = strategyService.DefinitionOfStrategy(investmentGoalComboBox, investmentHorizonComboBox, riskAccountingComboBox, expectedReturnComboBox, strategyAndConditions, userSelection);
        }

        private void riskAccountingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[2] = riskAccountingComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = strategyService.DefinitionOfStrategy(investmentGoalComboBox, investmentHorizonComboBox, riskAccountingComboBox, expectedReturnComboBox, strategyAndConditions, userSelection);
        }

        private void expectedReturnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[3] = expectedReturnComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = strategyService.DefinitionOfStrategy(investmentGoalComboBox, investmentHorizonComboBox, riskAccountingComboBox, expectedReturnComboBox, strategyAndConditions, userSelection);
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {          
            if (userSelection.Any(item => item == null) || capitalTextBox.Text == "")
            {
                MessageBox.Show("Сначала заполните все поля");
                return;
            }
            if (stockDataList.Count > 0)
            {
                SamplingStrategy(strategyTextBlock.Text);
                return;
            }

            progressBar.Visibility =Visibility;
            // Создаем объект Progress для обновления прогресс-бара
            var progress = new Progress<int>(value =>
            {
                // Обновляем прогресс-бар
                progressBar.Value = value;
            });

            await financeDataHandler.FillStockDataList(stockDataList);

            // Вызываем второй метод с обновлением прогресса
            await Task.Run(async () =>
            {
                await financeDataHandler.FillDataForCalculationsList(dataForCalculationsList, stockDataList, progress);
            });

            //прогнозирование доходности акции
            var profitabilityList = (from item in dataForCalculationsList
                                     group item by new { item.SecurityId, item.BoardID } into groupedData
                                     select new { SecurityId = groupedData.Key.SecurityId, BoardID = groupedData.Key.BoardID, ProfitabilityArray = groupedData.Select(x => x.Profitability).ToArray() }).ToList();

            foreach (var profitabilityData in profitabilityList)
            {
                double[] dailyReturns = profitabilityData.ProfitabilityArray;
                double alpha = strategyService.AutoExponentialSmoothing(dailyReturns);
                string securityId = profitabilityData.SecurityId;
                string boardId = profitabilityData.BoardID;

                // Применяем экспоненциальное сглаживание для прогнозирования и обновления данных
                strategyService.ExponentialSmoothingProfitability(stockDataList, dailyReturns, alpha, securityId, boardId);
            }

            //прогнозирование рисков акции
            var riskList = (from item in dataForCalculationsList
                            group item by new { item.SecurityId, item.BoardID } into groupedData
                            select new { SecurityId = groupedData.Key.SecurityId, BoardID = groupedData.Key.BoardID, RiskArray = groupedData.Select(x => x.Risk).ToArray() }).ToList();

            foreach (var riskData in riskList)
            {
                double[] dailyReturns = riskData.RiskArray;
                double alpha = strategyService.AutoExponentialSmoothing(dailyReturns);
                string securityId = riskData.SecurityId;
                string boardId = riskData.BoardID;

                // Применяем экспоненциальное сглаживание для прогнозирования и обновления данных
                strategyService.ExponentialSmoothingRisk(stockDataList, dailyReturns, alpha, securityId, boardId);
            }          
            progressBar.Visibility = Visibility.Hidden;

            SamplingStrategy(strategyTextBlock.Text);
        }

        private void SamplingStrategy(string strategy)
        {
            switch (strategy)
            {
                case "Консервативная":
                    capital = Convert.ToDouble(capitalTextBox.Text);
                    portfolios = PortfolioBuilder.BuildConservativePortfolio(stockDataList, capital);

                    // Вывод результатов в MessageBox
                    string message1 = "Инвестиционный портфель:\n\n";
                    foreach (var investment in portfolios)
                    {
                        message1 += $"Акция: {investment.SecurityName}\n";
                        message1 += $"ID: {investment.SecurityId}\n";
                        message1 += $"Количество: {investment.Quantity}\n";
                        message1 += $"Сумма инвестиций: {investment.TotalInvestment}\n\n";
                    }
                    MessageBox.Show(message1);
                    break;
                
                case "Сбалансированная":
                    MessageBox.Show("метод для сбланансир");
                    break;
                
                case "Агрессивная":
                    MessageBox.Show("метод для агрессивн");
                    break;
                
                case "Пассивная":
                    MessageBox.Show("метод для пассив"); ;
                    break;
                
                case "Активная":
                    MessageBox.Show("метод для актив"); ;
                    break;
               
                default:
                    MessageBox.Show("Какие то проблемы"); ;
                    break;
            }
        }
    }
}
