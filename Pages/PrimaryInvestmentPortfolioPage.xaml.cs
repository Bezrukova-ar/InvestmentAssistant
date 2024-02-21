using InvestmentAssistant.Model;
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
    public partial class PrimaryInvestmentPortfolioPage : Page
    {
        /// <summary> Экземпляр класса для заполнения данными combobox</summary>
        StrategyAndConditions strategyAndConditions = new StrategyAndConditions();
        /// <summary> Экземпляр класса для стратегий</summary>
        StrategyService strategyService = new StrategyService();

        string[] userSelection = { null, null, null, null };

        public PrimaryInvestmentPortfolioPage()
        {
            InitializeComponent();
            investmentGoalComboBox.ItemsSource = strategyAndConditions.InvestmentGoalList;
            investmentHorizonComboBox.ItemsSource = strategyAndConditions.InvestmentHorizonList;
            riskAccountingComboBox.ItemsSource = strategyAndConditions.RiskAccountingList;
            expectedReturnComboBox.ItemsSource = strategyAndConditions.ExpectedReturnList;
        }

        /// <summary> Ввод только тех значений, что являются числом </summary>
        private void capitalTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void investmentGoalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[0] = investmentGoalComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = strategyService.DefinitionOfStrategy(investmentGoalComboBox, investmentHorizonComboBox, riskAccountingComboBox, expectedReturnComboBox, strategyAndConditions, userSelection);
        }

        private void investmentHorizonComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[1] = investmentHorizonComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = strategyService.DefinitionOfStrategy(investmentGoalComboBox, investmentHorizonComboBox, riskAccountingComboBox, expectedReturnComboBox, strategyAndConditions, userSelection);
        }

        private void riskAccountingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[2] = riskAccountingComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = strategyService.DefinitionOfStrategy(investmentGoalComboBox, investmentHorizonComboBox, riskAccountingComboBox, expectedReturnComboBox, strategyAndConditions, userSelection);
        }

        private void expectedReturnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[3] = expectedReturnComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = strategyService.DefinitionOfStrategy(investmentGoalComboBox, investmentHorizonComboBox, riskAccountingComboBox, expectedReturnComboBox, strategyAndConditions, userSelection);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (userSelection.Any(item => item == null))
            {
                MessageBox.Show("Сначала заполните все поля");
                return;
            }
        }
    }
}
