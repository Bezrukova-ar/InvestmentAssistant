using InvestmentAssistant.Model.Strategy;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
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

                    investmentPortfolioDataGrid.Visibility = Visibility;
                    savePDFButton.Visibility = Visibility;
                    saveXLXSButton.Visibility = Visibility;
                    investmentPortfolioDataGrid.ItemsSource = portfolios;
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

        private void savePDFButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание и настройка диалогового окна выбора места сохранения
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF";

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Создание документа и писателя PDF
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                document.Open();

                // Установка шрифта с поддержкой кириллицы
                BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont);

                // Создание таблицы для результатов портфелей
                PdfPTable table = new PdfPTable(5);

                // Добавление заголовков столбцов в таблицу
                table.AddCell(new PdfPCell(new Phrase("Security ID", font)));
                table.AddCell(new PdfPCell(new Phrase("Security Name", font)));
                table.AddCell(new PdfPCell(new Phrase("Board ID", font)));
                table.AddCell(new PdfPCell(new Phrase("Quantity", font)));
                table.AddCell(new PdfPCell(new Phrase("Total Investment", font)));

                // Добавление данных портфелей в таблицу
                foreach (var portfolio in portfolios)
                {
                    table.AddCell(new PdfPCell(new Phrase(portfolio.SecurityId, font)));
                    table.AddCell(new PdfPCell(new Phrase(portfolio.SecurityName, font)));
                    table.AddCell(new PdfPCell(new Phrase(portfolio.BoardID, font)));
                    table.AddCell(new PdfPCell(new Phrase(portfolio.Quantity.ToString(), font)));
                    table.AddCell(new PdfPCell(new Phrase(portfolio.TotalInvestment.ToString(), font)));
                }

                // Добавление таблицы в документ
                document.Add(table);

                // Закрытие документа и остановка писателя PDF
                document.Close();
                writer.Close();

                // Открытие диалогового окна с сообщением об успешном сохранении
                System.Windows.Forms.MessageBox.Show("Результаты успешно сохранены в файл PDF.", "Успех", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        private void saveXLXSButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
