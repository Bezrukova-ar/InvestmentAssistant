using InvestmentAssistant.Model;
using InvestmentAssistant.Model.Chart;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InvestmentAssistant.Pages
{
    /// <summary>
    /// Класс представляет страницу в приложении, 
    /// специально предназначенную для отображения статистической информации, 
    /// связанной с финансовыми данными
    /// </summary>
    public partial class StatisticsPage : Page
    {
        /// <summary> Экземпляр класса для заполнения таблиц финансовыми данными </summary>
        FinanceDataHandler financeDataHandler = new FinanceDataHandler();
        /// <summary> Экземпляр класса для управления операциями с финансовыми данными </summary>
        SecurityService securityService = new SecurityService();

        /// <summary>  Словарь для хранения информации для расчета волатильности </summary>
        Dictionary<int, StockDataToCalculateVolatility> dataToCalculateVolatilityDictionary = new Dictionary<int, StockDataToCalculateVolatility>();
        /// <summary>  Словарь для хранения информации для построения свечного графика </summary>
        Dictionary<int, CandlestickData> candlestickChartDictionary = new Dictionary<int, CandlestickData>();
        /// <summary>  Словарь для хранения информации  для построения графика объема сделок </summary>
        Dictionary<int, SecurityTradingHistory> volumeTradeDictionary = new Dictionary<int, SecurityTradingHistory>();
        /// <summary>  Словарь для хранения информации  о ценных бумагах </summary>
        Dictionary<int, SharePriceTodayAndYesterday> priceChangeDictionary = new Dictionary<int, SharePriceTodayAndYesterday>();
        /// <summary>  Словарь для хранения информации о наиболее возросших акций </summary>
        Dictionary<string, List<SharePriceTodayAndYesterday>> risingStocksDictionary = new Dictionary<string, List<SharePriceTodayAndYesterday>>();
        /// <summary>  Словарь для хранения информации о наиболее упавших акций </summary>
        Dictionary<string, List<SharePriceTodayAndYesterday>> fallingStocksDictionary = new Dictionary<string, List<SharePriceTodayAndYesterday>>();

        /// <summary> Уникальный код ценной бумаги </summary>
        string symbol;
        /// <summary> Название ценной бумаги </summary>
        string nameSecurity;
        /// <summary> Дата начала построения графика </summary>
        DateTime startDate;
        /// <summary> Дата окончания построения графика </summary>
        DateTime endDate;
        /// <summary> Результат расчета стандартного отклонения </summary>
        string resultStandardDeviation;
        /// <summary> Результат расчета среднего истинного значения </summary>
        string resultAverageTrueRange;
        /// <summary> Результат расчета индекса волатильности</summary>
        string resultVolatilityIndex;
        /// <summary> Результат расчета среднего отклонения</summary>
        string resultAverageDeviation;

        public StatisticsPage()
        {
            InitializeComponent();
            autoComboBox.IsEditable = true;
            Loaded += StatisticsPage_Loaded;
        }

        private async void StatisticsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (priceChangeDictionary.Count == 0)
            {
                await financeDataHandler.FillThePriceChangeDictionary(priceChangeDictionary);
            }

            //Наиболее возросшие акции
            risingStocksDictionary = securityService.GetTopGainers(priceChangeDictionary);
            StringBuilder risingStocksStringBuilder = new StringBuilder();
            foreach (var pair in risingStocksDictionary)
            {
                risingStocksStringBuilder.AppendLine($"Режим торгов: {pair.Key}");
                foreach (var sharePrice in pair.Value)
                {
                    risingStocksStringBuilder.AppendLine($"Название: {sharePrice.SecurityName}\nИзменение: {Math.Round(sharePrice.PercentageChangeInValue, 2)}\n");
                }
                risingStocksStringBuilder.AppendLine();
            }
            TopRisingStocks.Text = risingStocksStringBuilder.ToString();

            //Наиболее упавшие акции
            fallingStocksDictionary = securityService.GetWorseLosers(priceChangeDictionary);
            StringBuilder fallingStringBuilder = new StringBuilder();
            foreach (var pair in fallingStocksDictionary)
            {
                fallingStringBuilder.AppendLine($"Режим торгов: {pair.Key}");
                foreach (var sharePrice in pair.Value)
                {
                    fallingStringBuilder.AppendLine($"Название: {sharePrice.SecurityName}\nИзменение: {Math.Round(sharePrice.PercentageChangeInValue, 2)}\n");
                }
                fallingStringBuilder.AppendLine();
            }
            TopFallingStocks.Text = fallingStringBuilder.ToString();
        }

        /// <summary> Обработчик события SelectedDateChanged, обеспечивает согласование выбранных дат
        /// в startDatePicker и endDatePicker, позволяя пользователю выбирать период времени, 
        /// который всегда составляет не менее 7 дней </summary>
        private void startDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (startDatePicker.SelectedDate == null)            
                return;
            
            endDatePicker.DisplayDateStart = startDatePicker.SelectedDate.Value.AddDays(7);
        }

        /// <summary> Обработчик события SelectedDateChanged, обеспечивает согласование выбранных дат
        /// в startDatePicker и endDatePicker, позволяя пользователю выбирать период времени, 
        /// который всегда составляет не менее 7 дней </summary>
        private void endDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (endDatePicker.SelectedDate == null)
                return;

            startDatePicker.DisplayDateEnd = endDatePicker.SelectedDate.Value.AddDays(-7);
        }

        ///<summary> Обработчик события PreviewKeyDown выбирает первый элемент в поле со списком при нажатии клавиши Enter</summary>
        private void autoComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                SelectFirstItemFromComboBox();
            }
        }

        /// <summary>
        /// Обработчик события PreviewTextInput, он добавляет новый введенный текст 
        /// к существующему и фильтрует список доступных акций
        /// для отображения только тех, которые содержат введенный текст
        /// </summary>
        private void autoComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string newText = autoComboBox.Text + e.Text;
            var filteredSecurities = MainWindow.securitiesHashTable.Values
                .Cast<NameOfSecurities>()
                .Where(security =>
                    security.SecurityName.ToLower().Contains(newText.ToLower()))
                .OrderBy(security => security.SecurityName.IndexOf(newText, StringComparison.OrdinalIgnoreCase))
                .Select(security => security.SecurityName)
                .ToList();
            autoComboBox.ItemsSource = filteredSecurities;
            autoComboBox.IsDropDownOpen = true;
            autoComboBox.Text = newText;
            e.Handled = true;
        }

        private void autoComboBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateSymbolFromSelectedSecurity();
        }
        /// <summary>
        /// Выбирает первый элемент из ComboBox и выполняет дополнительные действия на основе выполнения условия
        /// </summary>
        private void SelectFirstItemFromComboBox()
        {
            if (autoComboBox.SelectedItem != null)
            {
                // Получаем выбранный элемент
                string selectedSecurity = autoComboBox.SelectedItem as string;

                // Находим соответствующий элемент в securitiesHashTable
                NameOfSecurities selectedSecuritiesData = MainWindow.securitiesHashTable.Values
                    .Cast<NameOfSecurities>()
                    .FirstOrDefault(security => security.SecurityName == selectedSecurity);

                if (selectedSecuritiesData != null)
                {
                    symbol = selectedSecuritiesData.SecurityId.ToString();                   
                }

                // Закрываем выпадающий список
                autoComboBox.IsDropDownOpen = false;
            }
            else if (autoComboBox.ItemsSource != null && autoComboBox.ItemsSource.OfType<string>().Any())
            {
                autoComboBox.SelectedItem = autoComboBox.ItemsSource.OfType<string>().First();
                autoComboBox.IsDropDownOpen = false;
            }
            else
            {
                // Очищаем autoComboBox
                autoComboBox.Text = string.Empty;
            }          
        }

        ///<summary> Метод UpdateSymbolFromSelectedSecurity обновляет
        ///значение перменной symbol на основе выбора, сделанного в ComboBox </summary>
        private void UpdateSymbolFromSelectedSecurity()
        {
            if (autoComboBox.SelectedItem != null)
            {
                string selectedSecurity = autoComboBox.SelectedItem as string;
                NameOfSecurities selectedSecuritiesData = MainWindow.securitiesHashTable.Values
                    .Cast<NameOfSecurities>()
                    .FirstOrDefault(security => security.SecurityName == selectedSecurity);
                if (selectedSecuritiesData != null)
                {
                    symbol = selectedSecuritiesData.SecurityId.ToString();
                }
                autoComboBox.IsDropDownOpen = false;
            }
            else if (autoComboBox.ItemsSource != null && autoComboBox.ItemsSource.OfType<string>().Any())
            {
                // Если ни один элемент не выбран, но в ComboBox есть элементы, выбрать первый
                autoComboBox.SelectedItem = autoComboBox.ItemsSource.OfType<string>().First();
                UpdateSymbolFromSelectedSecurity();
            }
            else
            {
                autoComboBox.Text = string.Empty;
            }
        }

        private async void downloadStockPriceChart_Click(object sender, RoutedEventArgs e)
        {
            nameSecurity = autoComboBox.SelectedItem?.ToString();
            symbol = securityService.GetIdSecurityByName(nameSecurity);

            if (startDatePicker.SelectedDate == null || endDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Проверьте, ввели ли вы обе даты", "Внимание!");
                return;
            }

            startDate = (DateTime)startDatePicker.SelectedDate;
            endDate = (DateTime)endDatePicker.SelectedDate;

            if (symbol == null)
            {
                return;
            }           

            await financeDataHandler.FillCandlestickChartDictionary(symbol, startDate, endDate, candlestickChartDictionary);

            var plotModel = new CartesianChart { };
            var candlestickSeries = new CandleSeries
            {
                Values = new ChartValues<OhlcPoint>()
            };

            foreach (var entry in candlestickChartDictionary)
            {
                var candlestickData = entry.Value;
                candlestickSeries.Values.Add(new OhlcPoint
                {
                    High = (double)candlestickData.High,
                    Low = (double)candlestickData.Low,
                    Open = (double)candlestickData.Open,
                    Close = (double)candlestickData.Close
                });
            }

            plotModel.Series.Add(candlestickSeries);
            candlestickChart.Series = new SeriesCollection { candlestickSeries };
            candlestickChart.LegendLocation = LegendLocation.None;
            candlestickChart.Visibility = Visibility;

            await financeDataHandler.FillVolumeTradeDictionary(symbol, startDate, endDate, volumeTradeDictionary);

            volumeChart.Series.Clear(); // очищаем графики перед добавлением новых
            var uniqueBoardIDs = volumeTradeDictionary.Values.Cast<SecurityTradingHistory>().Select(x => x.BoardID).Distinct();

            foreach (string boardID in uniqueBoardIDs)
            {
                List<SecurityTradingHistory> valuesForBoardID = volumeTradeDictionary.Values.Cast<SecurityTradingHistory>().Where(x => x.BoardID == boardID).OrderBy(x => x.TradeDate).ToList();
                var series = new LineSeries
                {
                    Title = boardID,
                    Values = new ChartValues<double>(valuesForBoardID.Select(data => data.Volume)),
                };
                volumeChart.Series.Add(series);
                valuesForBoardID.Clear();
            }
            volumeChart.AxisX[0].Labels = volumeTradeDictionary.Values.Cast<SecurityTradingHistory>().Select(data => data.TradeDate.ToShortDateString()).Distinct().ToArray();
            volumeChart.Visibility = Visibility;

            // Расчет волатильности акции
            await financeDataHandler.FillStockDataToCalculateVolatilityDictionary(symbol, dataToCalculateVolatilityDictionary);

            //Стандартное отклонение
            resultStandardDeviation = "";
            resultStandardDeviation = securityService.CalculationOfStandardDeviation(dataToCalculateVolatilityDictionary);
            standardDeviationTextBlock.ToolTip = resultStandardDeviation;

            //Средний истинный диапазон
            resultAverageTrueRange = "";
            resultAverageTrueRange = securityService.AverageTrueRangeCalculation(dataToCalculateVolatilityDictionary);
            averageTrueRangeTextBlock.ToolTip = resultAverageTrueRange;

            //Индекс волатильности
            resultVolatilityIndex = "";
            resultVolatilityIndex = securityService.VolatilityIndexCalculation(dataToCalculateVolatilityDictionary);
            volatilityIndexTextBlock.ToolTip = resultVolatilityIndex;

            //Среднее отклонение
            resultAverageDeviation = "";
            resultAverageDeviation = securityService.CalculationOfAverageDeviation(dataToCalculateVolatilityDictionary);
            averageDeviationTextBlock.ToolTip = resultAverageDeviation;
        }

        ///<summary> Построение графика наиболее выросших акций </summary>
        private void barChartOfRisingStocks_Click(object sender, RoutedEventArgs e)
        {
            var chartData = risingStocksDictionary.SelectMany(x => x.Value.Select(y => new GainerData
            {
                SecurityName = y.SecurityName,
                PercentageChange = y.PercentageChangeInValue
            })).ToList();

            // Создание экземпляра SeriesCollection для хранения данных графика
            SeriesCollection series = new SeriesCollection();
            stockChart.Series.Clear();

            foreach (var data in chartData)
            {
                var columnSeries = new ColumnSeries
                {
                    Values = new ChartValues<double> { Math.Round(data.PercentageChange, 2) },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Y}%", // отображение процентного изменения
                    Title = data.SecurityName // установка названия акции для этого столбца
                };
                series.Add(columnSeries); // добавление столбца в коллекцию
                stockChart.Series.Add(columnSeries);
            }
        }

        ///<summary> Построение графика наиболее упавших акций </summary>
        private void barChartOfFallingStocks_Click(object sender, RoutedEventArgs e)
        {
            var chartData = fallingStocksDictionary.SelectMany(x => x.Value.Select(y => new GainerData
            {
                SecurityName = y.SecurityName,
                PercentageChange = y.PercentageChangeInValue
            })).ToList();

            // Создание экземпляра SeriesCollection для хранения данных графика
            SeriesCollection series = new SeriesCollection();
            stockChart.Series.Clear();

            foreach (var data in chartData)
            {
                var columnSeries = new ColumnSeries
                {
                    Values = new ChartValues<double> { Math.Round(data.PercentageChange, 2) },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Y}%", // отображение процентного изменения
                    Title = data.SecurityName // установка названия акции для этого столбца
                };
                series.Add(columnSeries); // добавление столбца в коллекцию
                stockChart.Series.Add(columnSeries);
            }
        }
    }
}
