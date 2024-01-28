using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
    /// Lógica de interacción para StatisticsPage.xaml
    /// </summary>
   
    public partial class StatisticsPage : Page
    {
        Methods methods = new Methods();


    


        public static Hashtable candlestickChartDataHash = new Hashtable();
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
        private void startDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //endDatePicker.DisplayDateStart = startDatePicker.SelectedDate;
            if (startDatePicker.SelectedDate != null)
            {
                endDatePicker.DisplayDateStart = startDatePicker.SelectedDate.Value.AddDays(7);
            }
        }

        private void endDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //startDatePicker.DisplayDateEnd = endDatePicker.SelectedDate;
            if (endDatePicker.SelectedDate != null)
            {
                startDatePicker.DisplayDateEnd = endDatePicker.SelectedDate.Value.AddDays(-7);
            }
        }
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
                // If no item is selected, but there are items in the ComboBox, select the first one
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
                symbol = methods.GetIdSecurityByName(nameSecurity);
                if (symbol == null)
                {
                    return; // прервать выполнение программы
                }
            }
            else
            {
                if (startDate != null && endDate != null)
                {
                    //тут должен выполняться запрос и строиться график
                    candlestickChartDataHash.Clear();
                    await methods.FillCandlestickChartDataHash(symbol, startDate, endDate, candlestickChartDataHash);
                    // Отображение значения хеш-таблицы в MessageBox
                    /* string message = "Хеш-таблица candlestickChartDataHash:\n";
                     foreach (var key in candlestickChartDataHash.Keys)
                     {
                         var candlestickData = (CandlestickData)candlestickChartDataHash[key];
                         message += $"Key: {key}, Value: {candlestickData.Open}, {candlestickData.Low}, {candlestickData.High}, {candlestickData.Close}\n"; //и так далее но уже с датой
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
