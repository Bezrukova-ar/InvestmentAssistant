using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace InvestmentAssistant.Pages
{
    /// <summary>
    /// Класс представляет страницу в приложении, 
    /// специально предназначенную для отображения статистической информации, 
    /// связанной с финансовыми данными
    /// </summary>
    public partial class StatisticsPage : Page
    {
        /// <summary> Экземпляр класса для управления операциями с финансовыми данными </summary>
        FinanceDataHandler financeDataHandler = new FinanceDataHandler();
        /// <summary>  Статическая хэш-таблица, которая будет хранить информацию для построения свечного графика </summary>
        public static Hashtable candlestickChartDataHash = new Hashtable();
        /// <summary>  Статическая хэш-таблица, которая будет хранить информацию для построения графика объема сделок </summary>
        public static Hashtable volumeTradeDataHash = new Hashtable();
        /// <summary> Уникальный код ценной бумаги </summary>
        public static string symbol;
        /// <summary> Название ценной бумаги </summary>
        public static string nameSecurity;
        /// <summary> Дата начала построения графика </summary>
        public static DateTime startDate;
        /// <summary> Дата окончания построения графика </summary>
        public static DateTime endDate;


        public StatisticsPage()
        {
            InitializeComponent();
            autoComboBox.IsEditable = true;
        }
        /// <summary> Обработчик события SelectedDateChanged, обеспечивает согласование выбранных дат
        /// в startDatePicker и endDatePicker, позволяя пользователю выбирать период времени, 
        /// который всегда составляет не менее 7 дней </summary>
        private void startDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (startDatePicker.SelectedDate != null)
            {
                endDatePicker.DisplayDateStart = startDatePicker.SelectedDate.Value.AddDays(7);
            }
        }
        /// <summary> Обработчик события SelectedDateChanged, обеспечивает согласование выбранных дат
        /// в startDatePicker и endDatePicker, позволяя пользователю выбирать период времени, 
        /// который всегда составляет не менее 7 дней </summary>
        private void endDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (endDatePicker.SelectedDate != null)
            {
                startDatePicker.DisplayDateEnd = endDatePicker.SelectedDate.Value.AddDays(-7);
            }
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

            /* string newText = autoComboBox.Text + e.Text;
             var filteredSecurities = MainWindow.securitiesHashTable.Values
                 .Cast<NameOfSecurities>()
                 .Where(security =>
                     security.SecurityName.ToLower().Contains(newText.ToLower()))
                 .Select(security => security.SecurityName)
                 .ToList();
             autoComboBox.ItemsSource = filteredSecurities;
             autoComboBox.IsDropDownOpen = true;
             autoComboBox.Text = newText;
             e.Handled = true;      */

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
            startDate = (DateTime)(startDatePicker.SelectedDate != null ? startDatePicker.SelectedDate : null);
            endDate = (DateTime)(endDatePicker.SelectedDate != null ? endDatePicker.SelectedDate : null);
            nameSecurity = autoComboBox.SelectedItem?.ToString();          

            if (symbol == null)
            {
                symbol = financeDataHandler.GetIdSecurityByName(nameSecurity);
                if (symbol == null)
                {
                    return; // прервать выполнение программы
                }
            }
            else
            {
                if (startDate != null && endDate != null)
                {
                    
                    candlestickChartDataHash.Clear();
                    volumeTradeDataHash.Clear();

                    await financeDataHandler.FillCandlestickChartDataHash(symbol, startDate, endDate, candlestickChartDataHash);
                    // Отображение значения хеш-таблицы в MessageBox
                    /*string message = "Хеш-таблица candlestickChartDataHash:\n";
                    foreach (var key in candlestickChartDataHash.Keys)
                    {
                        var candlestickData = (CandlestickData)candlestickChartDataHash[key];
                        message += $"Key: {key}, Value: {candlestickData.Open}, {candlestickData.Low}, {candlestickData.High}, {candlestickData.Close}, {candlestickData.StartDate}\n"; //и так далее но уже с датой
                    }
                    MessageBox.Show(message, "Значение хеш-таблицы", MessageBoxButton.OK, MessageBoxImage.Information);*/
                    
                    var plotModel = new CartesianChart{};

                     // Создание серии свечей
                     var candlestickSeries = new CandleSeries
                     {
                         Values = new ChartValues<OhlcPoint>()
                     };

                     // Сортировка элементов хеш-таблицы по ключу
                     var sortedEntries = candlestickChartDataHash.Cast<DictionaryEntry>().OrderBy(entry => (int)entry.Key);

                     // Заполнение серии данными из отсортированной хеш-таблицы
                     foreach (DictionaryEntry entry in sortedEntries)
                     {
                         var candlestickData = (CandlestickData)entry.Value;
                         candlestickSeries.Values.Add(new OhlcPoint
                         {
                             High = (double)candlestickData.High,
                             Low = (double)candlestickData.Low,
                             Open = (double)candlestickData.Open,
                             Close = (double)candlestickData.Close
                         });
                     }

                     // Добавление серии к модели
                     plotModel.Series.Add(candlestickSeries);

                    // Привязка модели к CartesianChart
                    candlestickChart.Series = new SeriesCollection { candlestickSeries };
                    candlestickChart.LegendLocation = LegendLocation.None;
                    candlestickChart.Visibility = Visibility;


                    await financeDataHandler.FillVolumeTradeDataHash(symbol, startDate, endDate, volumeTradeDataHash);
                    // Отображение значения хеш-таблицы в MessageBox
                    /*string message = "Хеш-таблица volumeTradeDataHash:\n";
                    foreach (var key in volumeTradeDataHash.Keys)
                    {
                        var securityTradingHistory = (SecurityTradingHistory)volumeTradeDataHash[key];
                        message += $"Key: {key}, Value: {securityTradingHistory.BoardID}, {securityTradingHistory.TradeDate}, {securityTradingHistory.NumTrade}, {securityTradingHistory.Volume}\n"; 
                    }
                    MessageBox.Show(message, "Значение хеш-таблицы", MessageBoxButton.OK, MessageBoxImage.Information);*/

                }
                else
                {
                    MessageBox.Show("Вы не ввели даты", "Внимание!");
                }
            }
        }      
    }
}
