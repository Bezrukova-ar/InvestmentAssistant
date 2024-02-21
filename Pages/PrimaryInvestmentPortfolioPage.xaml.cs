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

        string[] userSelection = { "null", "null", "null", "null" };
        //int[,] matches = new int[5,4];

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
            strategyTextBlock.Text = DefinitionOfStrategy();
        }

        private void investmentHorizonComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[1] = investmentHorizonComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = DefinitionOfStrategy();
        }

        private void riskAccountingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[2] = riskAccountingComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = DefinitionOfStrategy();
        }

        private void expectedReturnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userSelection[3] = expectedReturnComboBox.SelectedItem.ToString();
            strategyTextBlock.Text = DefinitionOfStrategy();
        }

        /// <summary> Метод выбора стратегии (потом куда нибудь выкину) </summary>
        public string DefinitionOfStrategy()
        {
            string result;
            if (investmentGoalComboBox.SelectedItem == null || investmentHorizonComboBox.SelectedItem == null || riskAccountingComboBox.SelectedItem == null || expectedReturnComboBox.SelectedItem == null )
            {
                return result ="Заполниет все поля" ;
            }
            int[,] matches = new int[strategyAndConditions.StrategyAndConditionsList.Count, userSelection.Length];

            for (int i = 0; i < strategyAndConditions.StrategyAndConditionsList.Count; i++)
            {
                Strategy strategy = strategyAndConditions.StrategyAndConditionsList[i];
                matches[i, 0] = strategy.InvestmentGoal == userSelection[0] ? 1 : 0;
                matches[i, 1] = strategy.InvestmentHorizon == userSelection[1] ? 1 : 0;
                matches[i, 2] = strategy.RiskAccounting == userSelection[2] ? 1 : 0;
                matches[i, 3] = strategy.ExpectedReturn == userSelection[3] ? 1 : 0;
            }

            int maxMatches = 0;
            int bestMatchIndex = -1;

            for (int i = 0; i < strategyAndConditions.StrategyAndConditionsList.Count; i++)
            {
                int currentMatches = 0;
                for (int j = 0; j < userSelection.Length; j++)
                {
                    currentMatches += matches[i, j];
                }

                if (currentMatches > maxMatches)
                {
                    maxMatches = currentMatches;
                    bestMatchIndex = i;
                }
            }
           
            if (bestMatchIndex != -1)
            {
                Strategy bestMatchedStrategy = strategyAndConditions.StrategyAndConditionsList[bestMatchIndex];
                result = bestMatchedStrategy.Name;
            }
            else
            {
                result = "Подходящая стратегия не найдена";
            }

            return result;
        }
    }
}
