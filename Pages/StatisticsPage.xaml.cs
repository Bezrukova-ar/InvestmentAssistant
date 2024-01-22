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

        private void autoCompleteBox_Populating(object sender, PopulatingEventArgs e)
        {
            string searchText = autoCompleteBox.Text.ToLower();

             var filteredSecurities = MainWindow.securitiesHashTable.Values
                 .Cast<NameOfSecurities>()
                 .Where(security =>
                     security.SecurityName.ToLower().Contains(searchText) ||
                     security.LatinName.ToLower().Contains(searchText))
                 .Select(security => security.SecurityName)
                 .Take(10) // Ограничение до 10 элементов
                 .ToList();
             autoCompleteBox.ItemsSource = filteredSecurities;
        }

        private void autoCompleteBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectFirstItemFromAutoComplete();
        }       
    }
}
/*private void AutoCompleteBox_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchText = autoCompleteBox.Text.ToLower(); // Преобразование текста в нижний регистр для регистронезависимого сравнения
                if (MainWindow.securitiesHashTable != null)
                {
                    // Фильтрация данных в AutoCompleteBox на основе введенного текста
                    var filteredSecurities = MainWindow.securitiesHashTable.Values
                        .Cast<NameOfSecurities>()
                        .Where(security =>
                        security.SecurityName.ToLower().Contains(searchText) ||
                        security.LatinName.ToLower().Contains(searchText))
                       .Select(security => security.SecurityId)
                       .ToList();
                }
                else
                { 
                    // Обработка случая, когда securitiesDictionary пуста или не инициализирована
                    autoCompleteBox.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                // Обработка возможных исключений
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }*/