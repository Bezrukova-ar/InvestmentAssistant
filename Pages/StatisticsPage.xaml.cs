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
        public StatisticsPage()
        {
            InitializeComponent();
                 
        }

        private void AutoCompleteBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Обработка нажатия клавиши Enter
                e.Handled = true; // Предотвращение дальнейших событий клавиши Enter
                SelectFirstItemFromAutoComplete();
            }
        }
        private void SelectFirstItemFromAutoComplete()
        {
            if (autoCompleteBox.SelectedItem != null)
            {
                // Если выбран элемент, просто закрываем AutoCompleteBox
                autoCompleteBox.IsDropDownOpen = false;
            }
            else if (autoCompleteBox.ItemsSource != null && autoCompleteBox.ItemsSource.OfType<string>().Any())
            {
                // Если есть элементы в источнике данных AutoCompleteBox, выбираем первый и закрываем AutoCompleteBox
                autoCompleteBox.SelectedItem = autoCompleteBox.ItemsSource.OfType<string>().First();
                autoCompleteBox.IsDropDownOpen = false;
            }
            else
            {
                // Очищаем AutoCompleteBox, если нет элементов в источнике данных
                autoCompleteBox.Text = string.Empty;
            }
        }

        private void AutoCompleteBox_TextChanged(object sender, RoutedEventArgs e)
        {
           /* try
            {
                string searchText = autoCompleteBox.Text;

                if (methods.securitiesDictionary != null && methods.securitiesDictionary.Count > 0)
                {
                    // Фильтрация данных в AutoCompleteBox на основе введенного текста
                    var filteredSecurities = methods.securitiesDictionary.Values
                        .Where(security =>
                            security.SecurityId.ToLower().Contains(searchText.ToLower()) ||
                            security.SecurityName.ToLower().Contains(searchText.ToLower()))
                        .Select(security => security.SecurityId)
                        .ToList();

                    autoCompleteBox.ItemsSource = filteredSecurities;
                }
                else
                {
                    // Обработка случая, когда securitiesDictionary пуста или не инициализирована
                    autoCompleteBox.ItemsSource = null;
                }

                autoCompleteBox.IsDropDownOpen = true;
            }
            catch (Exception ex)
            {
                // Обработка возможных исключений
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
            string searchText = autoCompleteBox.Text;

             // Фильтрация данных в AutoCompleteBox на основе введенного текста
             var filteredSecurities = methods.securitiesDictionary.Values
                 .Where(security =>
                     security.SecurityName.ToLower().Contains(searchText.ToLower()) ||
                     security.LatinName.ToLower().Contains(searchText.ToLower()))
                 .Select(security => security.SecurityId)
                 .ToList();

             autoCompleteBox.ItemsSource = filteredSecurities;
             autoCompleteBox.IsDropDownOpen = true;
        }
    }
}
