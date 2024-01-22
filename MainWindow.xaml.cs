using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace InvestmentAssistant
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Methods methods = new Methods();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
     
        /// <summary>
        /// Кнопка закрытия окна
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Кнопка изменения размера окна
        /// </summary>
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Навигация по приложению
        /// </summary>
        private void rdHome_Click(object sender, RoutedEventArgs e)
        {
            

            PagesNavigation.Navigate(new Uri("Pages/StatisticsPage.xaml", UriKind.RelativeOrAbsolute));
        }
        private void rdSounds_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new Uri("Pages/PrimaryInvestmentPortfolioPage.xaml", UriKind.RelativeOrAbsolute));
        }
        private void rdNotes_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new Uri("Pages/InvestmentPortfolioOptimizationPage.xaml", UriKind.RelativeOrAbsolute));
        }
        private void rdPayment_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new Uri("Pages/HandbookPage.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Вызов метода для загрузки данных о ценных бумагах
        /// </summary>
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await methods.LoadedData();
            /*await Task.Run(() =>
            {
                methods.LoadedData();
            });*/

        }

    }
}
