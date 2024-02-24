using InvestmentAssistant.Model.Strategy;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace InvestmentAssistant.Pages
{
    public partial class InvestmentPortfolioOptimizationPage : Page
    {
        PortfolioOptimization portfolioOptimization = new PortfolioOptimization();
        /// <summary> Экземпляр класса для заполнения таблиц финансовыми данными </summary>
        FinanceDataHandler financeDataHandler = new FinanceDataHandler();
        /// <summary> Экземпляр класса для стратегий</summary>
        StrategyService strategyService = new StrategyService();

        /// <summary> Инвестиционный портфель имеющийся</summary>
        List<PortfolioAsset> portfolioList = new List<PortfolioAsset>();
        /// <summary> Инвестиционный портфель оптимизированный</summary>
        List<PortfolioAsset> optimizedPortfolio = new List<PortfolioAsset>();

        public InvestmentPortfolioOptimizationPage()
        {
            InitializeComponent();
        }

        /// <summary> Загрузка данных из файла </summary>
        private void loadDataButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";

            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo file = new FileInfo(openFileDialog.FileName);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    portfolioList.Clear();
                    for (int rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
                    {
                        PortfolioAsset asset = new PortfolioAsset
                        {
                            SecurityId = worksheet.Cells[rowNumber, 1].Text,
                            SecurityName = worksheet.Cells[rowNumber, 2].Text,
                            BoardID = worksheet.Cells[rowNumber, 3].Text,
                            Quantity = int.Parse(worksheet.Cells[rowNumber, 4].Text),
                            TotalInvestment = double.Parse(worksheet.Cells[rowNumber, 5].Text),
                        };
                        portfolioList.Add(asset);
                    }

                    portfolioDataGrid.Columns.Clear(); // Очистить столбцы DataGrid

                    // Создание столбцов, исключая Weight и StockRisk
                    foreach (var property in typeof(PortfolioAsset).GetProperties())
                    {
                        if (property.Name != "Weight" && property.Name != "StockRisk")
                        {
                            DataGridTextColumn textColumn = new DataGridTextColumn();
                            textColumn.Header = property.Name;
                            textColumn.Binding = new Binding(property.Name);
                            portfolioDataGrid.Columns.Add(textColumn);
                        }
                    }
                    portfolioDataGrid.ItemsSource = portfolioList;
                }
            }
        }

        /// <summary> Оптимизация имеющегося портфеля</summary>
        private async void portfolioOptimizationButton_Click(object sender, RoutedEventArgs e)
        {
            if (PrimaryInvestmentPortfolioPage.stockDataList.Count > 0)
            {
                optimizedPortfolio = portfolioOptimization.OptimizePortfolio(portfolioList, PrimaryInvestmentPortfolioPage.stockDataList);
                portfolioDataGrid.ItemsSource = optimizedPortfolio;
                return;
            }
            if (portfolioList.Count== 0)
            {
                MessageBox.Show("Сначала заполните таблицу данными, загрузив файл");
                return;
            }

            portfolioDataGrid.Visibility = Visibility.Hidden;
            progressBar.Visibility = Visibility;
            // Создаем объект Progress для обновления прогресс-бара
            var progress = new Progress<int>(value =>
            {
                // Обновляем прогресс-бар
                progressBar.Value = value;
            });

            await financeDataHandler.FillStockDataList(PrimaryInvestmentPortfolioPage.stockDataList);

            // Вызываем второй метод с обновлением прогресса
            await Task.Run(async () =>
            {
                await financeDataHandler.FillDataForCalculationsList(PrimaryInvestmentPortfolioPage.dataForCalculationsList, PrimaryInvestmentPortfolioPage.stockDataList, progress);
            });

            //прогнозирование доходности акции
            var profitabilityList = (from item in PrimaryInvestmentPortfolioPage.dataForCalculationsList
                                     group item by new { item.SecurityId, item.BoardID } into groupedData
                                     select new { SecurityId = groupedData.Key.SecurityId, BoardID = groupedData.Key.BoardID, ProfitabilityArray = groupedData.Select(x => x.Profitability).ToArray() }).ToList();

            foreach (var profitabilityData in profitabilityList)
            {
                double[] dailyReturns = profitabilityData.ProfitabilityArray;
                double alpha = strategyService.AutoExponentialSmoothing(dailyReturns);
                string securityId = profitabilityData.SecurityId;
                string boardId = profitabilityData.BoardID;

                // Применяем экспоненциальное сглаживание для прогнозирования и обновления данных
                strategyService.ExponentialSmoothingProfitability(PrimaryInvestmentPortfolioPage.stockDataList, dailyReturns, alpha, securityId, boardId);
            }

            //прогнозирование рисков акции
            var riskList = (from item in PrimaryInvestmentPortfolioPage.dataForCalculationsList
                            group item by new { item.SecurityId, item.BoardID } into groupedData
                            select new { SecurityId = groupedData.Key.SecurityId, BoardID = groupedData.Key.BoardID, RiskArray = groupedData.Select(x => x.Risk).ToArray() }).ToList();

            foreach (var riskData in riskList)
            {
                double[] dailyReturns = riskData.RiskArray;
                double alpha = strategyService.AutoExponentialSmoothing(dailyReturns);
                string securityId = riskData.SecurityId;
                string boardId = riskData.BoardID;

                // Применяем экспоненциальное сглаживание для прогнозирования и обновления данных
                strategyService.ExponentialSmoothingRisk(PrimaryInvestmentPortfolioPage.stockDataList, dailyReturns, alpha, securityId, boardId);
            }
            progressBar.Visibility = Visibility.Hidden;

            //Использование метода оптимизации
            optimizedPortfolio = portfolioOptimization.OptimizePortfolio(portfolioList, PrimaryInvestmentPortfolioPage.stockDataList);
            //Отображение таблицы
            portfolioDataGrid.ItemsSource = optimizedPortfolio;
            portfolioDataGrid.Visibility = Visibility;
        }

        /// <summary> Сохранение файла</summary>
        private void saveXLSXButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
