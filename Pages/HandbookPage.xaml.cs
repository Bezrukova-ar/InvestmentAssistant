using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvestmentAssistant.Pages
{
    public partial class HandbookPage : Page
    {
        public HandbookPage()
        {
            InitializeComponent();
            string text1 = "В программе, в разделе статистики выбирается акция для анализа, " +
                "по которой строится свечной график за определенный период времени. " +
                "Также отображается диаграмма объема торгов для данной акции. " +
                "На панели статистики также присутствует список топ-падающих и топ-растущих акций," +
                " который представлен в виде диаграммы для наглядного сравнения изменений на рынке.";
            string text2 = "На основе выбранных данных в выпадающих списках определяется наиболее подходящая стратегия. " +
                "С применением метода Макровица, который является статистическим методом для прогнозирования финансовых рынков " +
                "на основе корреляций между активами, формируется портфель в соответствии с выбранной стратегией. " +
                "В рамках доступного капитала производится отбор подходящих активов. После этого генерируется список активов," +
                " который можно сохранить в форматах PDF и XLSX.";
            string text3 = "Для улучшения уже имеющегося портфеля инвестиций применяется метод Шарпа. " +
                "Метод Шарпа используется для оценки доходности инвестиций относительно риска. " +
                "Для начала необходимо загрузить файл формата .xlsx, затем нажать кнопку \"Оптимизировать\". " +
                "Программа применяет алгоритм Шарпа для оптимизации портфеля, результаты представляются визуально. " +
                "После этого вы можете сохранить данные в файле формата .xlsx.";
            statisticTextBlock.Text = text1;
            primaryPortfolioTextBlock.Text = text2;
            optimizationPortfolioTextBlock.Text = text3;
        }     
    }
}
