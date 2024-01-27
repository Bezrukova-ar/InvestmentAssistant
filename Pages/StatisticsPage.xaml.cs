using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InvestmentAssistant.Pages
{
    /// <summary>
    /// Lógica de interacción para StatisticsPage.xaml
    /// </summary>
   
    public partial class StatisticsPage : Page
    {
        Methods methods = new Methods();

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
                    /*List<CandlestickData> candlestickDataList = await finance.GetCandlestickData(symbol, startDate, endDate);

                    // Далее вы можете использовать полученные данные, например:
                    foreach (var candlestickData in candlestickDataList)
                    {
                        MessageBox.Show($"Trade Date: {candlestickData.TradeDate}, Open: {candlestickData.Open}, Low: {candlestickData.Low}, High: {candlestickData.High}, Close: {candlestickData.Close}");
                    }*/
                    
                }
                else
                {
                    MessageBox.Show("Вы не ввели даты", "Внимание!");
                }
            }
        }      
    }
}
