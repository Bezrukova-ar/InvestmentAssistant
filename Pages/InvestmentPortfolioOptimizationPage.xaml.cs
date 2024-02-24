using Microsoft.Win32;
using OfficeOpenXml;
using System;
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
        PrimaryInvestmentPortfolioPage primaryInvestmentPortfolio = new PrimaryInvestmentPortfolioPage();
        /// <summary> Экземпляр класса для заполнения таблиц финансовыми данными </summary>
        FinanceDataHandler financeDataHandler = new FinanceDataHandler();
        /// <summary> Экземпляр класса для стратегий</summary>
        StrategyService strategyService = new StrategyService();

        /// <summary> Данные портфеля</summary>
        DataTable portfolioDataTable = new DataTable();

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

                // Установите свойство LicenseContext перед использованием EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    

                    foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                    {
                        portfolioDataTable.Columns.Add(firstRowCell.Text);
                    }

                    for (int rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
                    {
                        var wsRow = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];
                        DataRow row = portfolioDataTable.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }

                    portfolioDataGrid.Columns.Clear(); // Очистить столбцы DataGrid

                    //привязка данныъ с дататейбл к датагриду
                    foreach (DataColumn column in portfolioDataTable.Columns)
                    {
                        DataGridTextColumn textColumn = new DataGridTextColumn();
                        textColumn.Header = column.ColumnName;
                        textColumn.Binding = new Binding(column.ColumnName);
                        portfolioDataGrid.Columns.Add(textColumn);
                    }

                    portfolioDataGrid.ItemsSource = portfolioDataTable.DefaultView;
                }
            }
            portfolioDataTable.Clear();
        }

        /// <summary> Оптимизация имеющегося портфеля</summary>
        private async void portfolioOptimizationButton_Click(object sender, RoutedEventArgs e)
        {
            if (primaryInvestmentPortfolio.stockDataList.Count > 0)
            {
                //тут тоже метод оптимизации

                //Отображение таблицы
                portfolioDataGrid.Visibility = Visibility;
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

            await financeDataHandler.FillStockDataList(primaryInvestmentPortfolio.stockDataList);

            // Вызываем второй метод с обновлением прогресса
            await Task.Run(async () =>
            {
                await financeDataHandler.FillDataForCalculationsList(primaryInvestmentPortfolio.dataForCalculationsList, primaryInvestmentPortfolio.stockDataList, progress);
            });

            //прогнозирование доходности акции
            var profitabilityList = (from item in primaryInvestmentPortfolio.dataForCalculationsList
                                     group item by new { item.SecurityId, item.BoardID } into groupedData
                                     select new { SecurityId = groupedData.Key.SecurityId, BoardID = groupedData.Key.BoardID, ProfitabilityArray = groupedData.Select(x => x.Profitability).ToArray() }).ToList();

            foreach (var profitabilityData in profitabilityList)
            {
                double[] dailyReturns = profitabilityData.ProfitabilityArray;
                double alpha = strategyService.AutoExponentialSmoothing(dailyReturns);
                string securityId = profitabilityData.SecurityId;
                string boardId = profitabilityData.BoardID;

                // Применяем экспоненциальное сглаживание для прогнозирования и обновления данных
                strategyService.ExponentialSmoothingProfitability(primaryInvestmentPortfolio.stockDataList, dailyReturns, alpha, securityId, boardId);
            }

            //прогнозирование рисков акции
            var riskList = (from item in primaryInvestmentPortfolio.dataForCalculationsList
                            group item by new { item.SecurityId, item.BoardID } into groupedData
                            select new { SecurityId = groupedData.Key.SecurityId, BoardID = groupedData.Key.BoardID, RiskArray = groupedData.Select(x => x.Risk).ToArray() }).ToList();

            foreach (var riskData in riskList)
            {
                double[] dailyReturns = riskData.RiskArray;
                double alpha = strategyService.AutoExponentialSmoothing(dailyReturns);
                string securityId = riskData.SecurityId;
                string boardId = riskData.BoardID;

                // Применяем экспоненциальное сглаживание для прогнозирования и обновления данных
                strategyService.ExponentialSmoothingRisk(primaryInvestmentPortfolio.stockDataList, dailyReturns, alpha, securityId, boardId);
            }
            progressBar.Visibility = Visibility.Hidden;

            //Использование метода оптимизации

            //Отображение таблицы
            portfolioDataGrid.Visibility = Visibility;
        }
    }
}
