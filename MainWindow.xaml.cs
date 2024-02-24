using InvestmentAssistant.Pages;
using System;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;

namespace InvestmentAssistant
{
    /// <summary>
    /// Класс MainWindow представляет главное окно приложения
    /// и отвечает за инициализацию ключевых компонентов
    /// и обработку взаимодействия с пользователем
    /// </summary>
    public partial class MainWindow : Window
    {
        private StatisticsPage statisticsPage = new StatisticsPage();
        private PrimaryInvestmentPortfolioPage primaryInvestmentPortfolioPage = new PrimaryInvestmentPortfolioPage();
        private InvestmentPortfolioOptimizationPage investmentPortfolioOptimizationPage = new InvestmentPortfolioOptimizationPage();
        private HandbookPage handbookPage = new HandbookPage();

        /// <summary> Экземпляр класса для управления операциями с финансовыми данными </summary>
        FinanceDataHandler financeDataHandler = new FinanceDataHandler();
        /// <summary>  Статическая хэш-таблица, которая будет хранить информацию о ценных бумагах </summary>
        public static Hashtable securitiesHashTable = new Hashtable();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        /// <summary> Перетаскивание окна </summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        /// <summary> Перетаскивание окна </summary>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point position = Mouse.GetPosition(this);
                Left = position.X + Left - Mouse.GetPosition(this).X;
                Top = position.Y + Top - Mouse.GetPosition(this).Y;
            }
        }

        /// <summary> Кнопка закрытия окна </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary> Кнопка изменения размера окна </summary>
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        /// <summary> Кнопка изменения размера окна </summary>
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary> Навигация по приложению, открытие страницы со статистикой </summary>
        private void rdStatistics_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Content = statisticsPage;
        }

        /// <summary> Навигация по приложению, открытие страницы для формирования первичного портфеля </summary>
        private void rdPrimaryInvestmentPortfolio_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Content = primaryInvestmentPortfolioPage;
        }

        /// <summary> Навигация по приложению, открытие страницы для оптимизации имеющегося портфеля </summary>
        private void rdInvestmentPortfolioOptimization_Click(object sender, RoutedEventArgs e)
        {         
            PagesNavigation.Content = investmentPortfolioOptimizationPage;
        }

        /// <summary> Навигация по приложению, открытие страницы со справочной информацией </summary>
        private void rdHandbook_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Content = handbookPage;
        }

        /// <summary> Вызов метода для загрузки данных о ценных бумагах </summary>
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {               
            // Проверяем доступность интернет-соединения
            if (!CheckInternetConnection())
            {
                MessageBox.Show("Отсутствует интернет-соединение. программа будет закрыта принудительно");
                this.Close();
            }

            await financeDataHandler.FillSecuritiesHashTable();
        }

        /// <summary> Проверка интернет соединения </summary>
        public bool CheckInternetConnection()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString("http://www.google.com/generate_204");
                    return true;
                }
            }
            catch
            {
                return false; 
            }
        }
    }
}
