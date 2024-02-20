using InvestmentAssistant.Model;
using InvestmentAssistant.Model.Chart;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>  словарь, я устала писать диплом</summary>
        Dictionary<int, StockDataToCalculateVolatility> dataToCalculateVolatilityDictionary = new Dictionary<int, StockDataToCalculateVolatility>();
        /// <summary>  Статическая хэш-таблица, которая будет хранить информацию для построения свечного графика </summary>
        Dictionary<int, CandlestickData> candlestickChartDistionary = new Dictionary<int, CandlestickData>();
        /// <summary>  Статическая хэш-таблица, которая будет хранить информацию для построения графика объема сделок </summary>
        Dictionary<int, SecurityTradingHistory> volumeTradeDistionary = new Dictionary<int, SecurityTradingHistory>();
        /// <summary>  Статическая хэш-таблица, которая будет хранить информацию о ценных бумагах </summary>
        Dictionary<int, SharePriceTodayAndYesterday> priceChangeDistionary = new Dictionary<int, SharePriceTodayAndYesterday>();

        /// <summary> Уникальный код ценной бумаги </summary>
        string symbol;
        /// <summary> Название ценной бумаги </summary>
        string nameSecurity;
        /// <summary> Дата начала построения графика </summary>
        DateTime startDate;
        /// <summary> Дата окончания построения графика </summary>
        DateTime endDate;


        public StatisticsPage()
        {
            InitializeComponent();
            autoComboBox.IsEditable = true;
            Loaded += StatisticsPage_Loaded;
        }

        private async void StatisticsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (priceChangeDistionary.Count == 0)
            {

                await financeDataHandler.FillThePriceChangeDictionary(priceChangeDistionary);
            }
            

            //Самые выросшие акции
            var topRisingStocksByBoard = priceChangeDistionary.Values
                 .Cast<SharePriceTodayAndYesterday>()
                 .GroupBy(x => x.BoardID)
                 .SelectMany(group => group.OrderByDescending(x => x.PercentageChangeInValue).Take(3))
                 .Select(x => $"Группа торгов: {x.BoardID}\nНазвание:{x.SecurityName}\n{Math.Round(x.PercentageChangeInValue, 2)}")
                 .ToList();
            TopRisingStocks.Text = string.Join(Environment.NewLine + Environment.NewLine, topRisingStocksByBoard);

            //самые упавшие акции
            var topFallingStocks = priceChangeDistionary.Values
                 .Cast<SharePriceTodayAndYesterday>()
                 .GroupBy(x => x.BoardID)
                 .SelectMany(group => group.OrderBy(x => x.PercentageChangeInValue).Take(3))
                 .Select(x => $"Группа торгов: {x.BoardID}\nНазвание:{x.SecurityName}\n{Math.Round(x.PercentageChangeInValue, 2)}")
                 .ToList();
            TopFallingStocks.Text += string.Join(Environment.NewLine + Environment.NewLine, topFallingStocks);
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

            if (startDatePicker.SelectedDate == null || endDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Проверьте, ввели ли вы обе даты", "Внимание!");
                return;
            }
            startDate = (DateTime)startDatePicker.SelectedDate;
            endDate = (DateTime)endDatePicker.SelectedDate;
            if (symbol == null)
            {
                symbol = securityService.GetIdSecurityByName(nameSecurity);
                if (symbol == null)
                {
                    return;
                }
            }
            else
            {
                symbol = securityService.GetIdSecurityByName(nameSecurity);

                dataToCalculateVolatilityDictionary.Clear();
                candlestickChartDistionary.Clear();
                volumeTradeDistionary.Clear();


                await financeDataHandler.FillCandlestickChartDictionary(symbol, startDate, endDate, candlestickChartDistionary);
                // Отображение значения хеш-таблицы в MessageBox
                /*string message = "Хеш-таблица candlestickChartDistionary:\n";
                foreach (var key in candlestickChartDistionary.Keys)
                {
                    var candlestickData = (CandlestickData)candlestickChartDistionary[key];
                    message += $"Key: {key}, Value: {candlestickData.Open}, {candlestickData.Low}, {candlestickData.High}, {candlestickData.Close}, {candlestickData.StartDate}\n"; //и так далее но уже с датой
                }
                MessageBox.Show(message, "Значение хеш-таблицы", MessageBoxButton.OK, MessageBoxImage.Information);*/

                var plotModel = new CartesianChart { };
                var candlestickSeries = new CandleSeries
                {
                    Values = new ChartValues<OhlcPoint>()
                };

                foreach (var entry in candlestickChartDistionary)
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

                await financeDataHandler.FillVolumeTradeDictionary(symbol, startDate, endDate, volumeTradeDistionary);
                volumeChart.Series.Clear(); // очищаем графики перед добавлением новых
                var uniqueBoardIDs = volumeTradeDistionary.Values.Cast<SecurityTradingHistory>().Select(x => x.BoardID).Distinct();
                foreach (string boardID in uniqueBoardIDs)
                {
                    List<SecurityTradingHistory> valuesForBoardID = volumeTradeDistionary.Values.Cast<SecurityTradingHistory>().Where(x => x.BoardID == boardID).OrderBy(x => x.TradeDate).ToList();
                    var series = new LineSeries
                    {
                        Title = boardID,
                        Values = new ChartValues<double>(valuesForBoardID.Select(data => data.Volume)),
                    };
                    volumeChart.Series.Add(series);
                    valuesForBoardID.Clear();
                }
                volumeChart.AxisX[0].Labels = volumeTradeDistionary.Values.Cast<SecurityTradingHistory>().Select(data => data.TradeDate.ToShortDateString()).Distinct().ToArray();
                volumeChart.Visibility = Visibility;


                // Расчет волатильности акции
                await financeDataHandler.FillStockDataToCalculateVolatilityDictionary(symbol, dataToCalculateVolatilityDictionary);


                //для стандартного отклонения
                string result = "";
                foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
                {
                    string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                    double sumSquaredDifferences = 0;
                    double averageClose = group.Average(d => d.Value.Close);
                    int numOfDays = group.Count();

                    foreach (var data in group)
                    {
                        sumSquaredDifferences += Math.Pow(data.Value.Close - averageClose, 2);
                    }

                    double volatility = Math.Sqrt(sumSquaredDifferences / numOfDays);

                    result += $"Стандартное отклонение для режима торгов {boardID}: { Math.Round(volatility, 5)}\n";
                }
                standardDeviationTextBlock.ToolTip = result;

                //для среднего истинного диапазона
                string result1 = "";
                foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
                {
                    string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                    double sumATR = 0;
                    int numOfDays = group.Count();
                    double previousClose = group.Select(x => x.Value.Close).FirstOrDefault();
                    foreach (var data in group)
                    {
                        double highLowDifference = data.Value.High - data.Value.Low;
                        double highPreviousCloseDifference = Math.Abs(data.Value.High - previousClose);
                        double lowPreviousCloseDifference = Math.Abs(data.Value.Low - previousClose);
                        double currentATR = Math.Max(highLowDifference, Math.Max(highPreviousCloseDifference, lowPreviousCloseDifference));
                        sumATR += currentATR;
                        previousClose = data.Value.Close;
                    }
                    double ATR = sumATR / numOfDays;
                    result1 += $"Средний истинный диапазон для режима торгов {boardID}: { Math.Round(ATR, 5)}\n";
                }
                averageTrueRangeTextBlock.ToolTip = result1;

                //индекс волатильности
                string result2 = "";
                foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
                {
                    string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                    double sumPercentageDifference = 0;
                    int numOfDays = group.Count();

                    foreach (var data in group)
                    {
                        double high = data.Value.High;
                        double low = data.Value.Low;

                        sumPercentageDifference += (high - low) / high;
                    }

                    double volatility = (sumPercentageDifference / numOfDays) * 100;
                    result2 += $"Индекс волатильности для режима торгов {boardID}: { Math.Round(volatility, 5)}\n";
                }
                volatilityIndexTextBlock.ToolTip = result2;

                //среднее отклонение
                string result3 = "";
                foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
                {
                    string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                    double sumAbsoluteDifferences = 0;
                    double averageClose = group.Average(d => d.Value.Close);
                    int numOfDays = group.Count();
                    foreach (var data in group)
                    {
                        sumAbsoluteDifferences += Math.Abs(data.Value.Close - averageClose);
                    }
                    double volatility = sumAbsoluteDifferences / numOfDays;
                    result3 += $"Среднее отклонение для режима торгов {boardID}: { Math.Round(volatility, 5)}\n";
                }

                averageDeviationTextBlock.ToolTip = result3;

                //if (startDatePicker.SelectedDate != null && endDatePicker.SelectedDate != null)
                //{
                //    startDate = (DateTime)startDatePicker.SelectedDate;
                //    endDate = (DateTime)endDatePicker.SelectedDate;
                //    if (symbol == null)
                //    {
                //        symbol = securityService.GetIdSecurityByName(nameSecurity);
                //        if (symbol == null)
                //        {
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        symbol = securityService.GetIdSecurityByName(nameSecurity);


                //        dataToCalculateVolatilityDictionary.Clear();


                //        candlestickChartDistionary.Clear();
                //        volumeTradeDistionary.Clear();


                //        await financeDataHandler.FillCandlestickChartDictionary(symbol, startDate, endDate, candlestickChartDistionary);
                //        // Отображение значения хеш-таблицы в MessageBox
                //        /*string message = "Хеш-таблица candlestickChartDistionary:\n";
                //        foreach (var key in candlestickChartDistionary.Keys)
                //        {
                //            var candlestickData = (CandlestickData)candlestickChartDistionary[key];
                //            message += $"Key: {key}, Value: {candlestickData.Open}, {candlestickData.Low}, {candlestickData.High}, {candlestickData.Close}, {candlestickData.StartDate}\n"; //и так далее но уже с датой
                //        }
                //        MessageBox.Show(message, "Значение хеш-таблицы", MessageBoxButton.OK, MessageBoxImage.Information);*/

                //        var plotModel = new CartesianChart { };
                //        var candlestickSeries = new CandleSeries
                //        {
                //            Values = new ChartValues<OhlcPoint>()
                //        };

                //        foreach (var entry in candlestickChartDistionary)
                //        {
                //            var candlestickData = entry.Value;
                //            candlestickSeries.Values.Add(new OhlcPoint
                //            {
                //                High = (double)candlestickData.High,
                //                Low = (double)candlestickData.Low,
                //                Open = (double)candlestickData.Open,
                //                Close = (double)candlestickData.Close
                //            });
                //        }

                //        plotModel.Series.Add(candlestickSeries);
                //        candlestickChart.Series = new SeriesCollection { candlestickSeries };
                //        candlestickChart.LegendLocation = LegendLocation.None;
                //        candlestickChart.Visibility = Visibility;



                //        await financeDataHandler.FillVolumeTradeDictionary(symbol, startDate, endDate, volumeTradeDistionary);
                //        volumeChart.Series.Clear(); // очищаем графики перед добавлением новых
                //        var uniqueBoardIDs = volumeTradeDistionary.Values.Cast<SecurityTradingHistory>().Select(x => x.BoardID).Distinct();
                //        foreach (string boardID in uniqueBoardIDs)
                //        {
                //            List<SecurityTradingHistory> valuesForBoardID = volumeTradeDistionary.Values.Cast<SecurityTradingHistory>().Where(x => x.BoardID == boardID).OrderBy(x => x.TradeDate).ToList();
                //            var series = new LineSeries
                //            {
                //                Title = boardID,
                //                Values = new ChartValues<double>(valuesForBoardID.Select(data => data.Volume)),
                //            };
                //            volumeChart.Series.Add(series);
                //            valuesForBoardID.Clear();
                //        }
                //        volumeChart.AxisX[0].Labels = volumeTradeDistionary.Values.Cast<SecurityTradingHistory>().Select(data => data.TradeDate.ToShortDateString()).Distinct().ToArray();
                //        volumeChart.Visibility = Visibility;





                //        // Расчет волатильности акции
                //        await financeDataHandler.FillStockDataToCalculateVolatilityDictionary(symbol, dataToCalculateVolatilityDictionary);


                //        //для стандартного отклонения
                //        string result = "";
                //        foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
                //        {
                //            string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                //            double sumSquaredDifferences = 0;
                //            double averageClose = group.Average(d => d.Value.Close);
                //            int numOfDays = group.Count();

                //            foreach (var data in group)
                //            {
                //                sumSquaredDifferences += Math.Pow(data.Value.Close - averageClose, 2);
                //            }

                //            double volatility = Math.Sqrt(sumSquaredDifferences / numOfDays);

                //            result += $"Стандартное отклонение для режима торгов {boardID}: { Math.Round(volatility, 5)}\n";
                //        }
                //        standardDeviationTextBlock.ToolTip = result;

                //        //для среднего истинного диапазона
                //        string result1 = "";
                //        foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
                //        {
                //            string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                //            double sumATR = 0;
                //            int numOfDays = group.Count();
                //            double previousClose = group.Select(x => x.Value.Close).FirstOrDefault();
                //            foreach (var data in group)
                //            {
                //                double highLowDifference = data.Value.High - data.Value.Low;
                //                double highPreviousCloseDifference = Math.Abs(data.Value.High - previousClose);
                //                double lowPreviousCloseDifference = Math.Abs(data.Value.Low - previousClose);
                //                double currentATR = Math.Max(highLowDifference, Math.Max(highPreviousCloseDifference, lowPreviousCloseDifference));
                //                sumATR += currentATR;
                //                previousClose = data.Value.Close;
                //            }
                //            double ATR = sumATR / numOfDays;
                //            result1 += $"Средний истинный диапазон для режима торгов {boardID}: { Math.Round(ATR, 5)}\n";
                //        }
                //        averageTrueRangeTextBlock.ToolTip = result1;

                //        //индекс волатильности
                //        string result2 = "";
                //        foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
                //        {
                //            string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                //            double sumPercentageDifference = 0;
                //            int numOfDays = group.Count();

                //            foreach (var data in group)
                //            {
                //                double high = data.Value.High;
                //                double low = data.Value.Low;

                //                sumPercentageDifference += (high - low) / high;
                //            }

                //            double volatility = (sumPercentageDifference / numOfDays) * 100;
                //            result2 += $"Индекс волатильности для режима торгов {boardID}: { Math.Round(volatility, 5)}\n";
                //        }
                //        volatilityIndexTextBlock.ToolTip = result2;

                //        //среднее отклонение
                //        string result3 = "";
                //        foreach (var group in dataToCalculateVolatilityDictionary.GroupBy(d => d.Value.BoardID))
                //        {
                //            string boardID = group.Select(x => x.Value.BoardID).FirstOrDefault();
                //            double sumAbsoluteDifferences = 0;
                //            double averageClose = group.Average(d => d.Value.Close);
                //            int numOfDays = group.Count();
                //            foreach (var data in group)
                //            {
                //                sumAbsoluteDifferences += Math.Abs(data.Value.Close - averageClose);
                //            }
                //            double volatility = sumAbsoluteDifferences / numOfDays;
                //            result3 += $"Среднее отклонение для режима торгов {boardID}: { Math.Round(volatility, 5)}\n";
                //        }

                //        averageDeviationTextBlock.ToolTip = result3;


                //    }
                //}
                //else
                //{
                //    MessageBox.Show("Проверьте, ввели ли вы обе даты", "Внимание!");
                //}          
            }
        }

        private void barChartOfRisingStocks_Click(object sender, RoutedEventArgs e)
        {

            // Выбираем три наиболее выросшие акции по каждому виду торгов
            var topGainers = priceChangeDistionary.Values
                 .Cast<SharePriceTodayAndYesterday>()
                 .GroupBy(x => x.BoardID)
                 .SelectMany(group => group.OrderByDescending(x => x.PercentageChangeInValue).Take(3))
                 .Select(x => new { x.BoardID, x.SecurityName, x.PercentageChangeInValue })
                 .ToList();


            // Преобразование данных topGainers в GainerData
            var chartData = topGainers.Select(x => new GainerData
            {
                SecurityName = x.SecurityName,
                PercentageChange = x.PercentageChangeInValue
            }).ToList();

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

        private void barChartOfFallingStocks_Click(object sender, RoutedEventArgs e)
        {
            // Выбираем три наиболее упавшие акции по каждому виду торгов
            var topGainers = priceChangeDistionary.Values
                 .Cast<SharePriceTodayAndYesterday>()
                 .GroupBy(x => x.BoardID)
                 .SelectMany(group => group.OrderBy(x => x.PercentageChangeInValue).Take(3))
                 .Select(x => new { x.BoardID, x.SecurityName, x.PercentageChangeInValue })
                 .ToList();


            // Преобразование данных topGainers в GainerData
            var chartData = topGainers.Select(x => new GainerData
            {
                SecurityName = x.SecurityName,
                PercentageChange = x.PercentageChangeInValue
            }).ToList();

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
