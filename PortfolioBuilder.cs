using InvestmentAssistant.Model.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentAssistant
{
    public class PortfolioBuilder
    {
        public static List<InvestmentPortfolio> BuildConservativePortfolio(List<StockData> stockDataList, double capital)
        {
            // Создаем матрицу ковариации
            double[,] covarianceMatrix = CalculateCovarianceMatrix(stockDataList);

            // Определяем веса акций с помощью алгоритма Марковица
            double[] weights = MarkowitzModernization(covarianceMatrix);

            // Создаем инвестиционный портфель
            List<InvestmentPortfolio> portfolio = new List<InvestmentPortfolio>();
            double sum = 0;
            for (int i = 0; i < stockDataList.Count; i++)
            {
                double investmentAmount = capital * weights[i];

                if (investmentAmount > 0)
                {
                    int quantity = (int)(investmentAmount / stockDataList[i].CurrentSharePrice);

                    if (quantity > 0)
                    {
                        InvestmentPortfolio investment = new InvestmentPortfolio
                        {
                            SecurityId = stockDataList[i].SecurityId,
                            SecurityName = stockDataList[i].SecurityName,
                            BoardID = stockDataList[i].BoardID,
                            Quantity = quantity,
                            TotalInvestment = quantity * stockDataList[i].CurrentSharePrice
                        };
                        portfolio.Add(investment);
                        sum += quantity * stockDataList[i].CurrentSharePrice;
                    }                  
                }
                if (sum >= capital) 
                    return portfolio;
            }

            return portfolio;
        }

        private static double[,] CalculateCovarianceMatrix(List<StockData> stockDataList)
        {
            int n = stockDataList.Count;
            double[,] covarianceMatrix = new double[n, n];

            // Расчет ковариации между парами акций
            for (int i = 0; i < n; i++)
            {
                covarianceMatrix[i, i] = stockDataList[i].StockRisk * stockDataList[i].StockRisk;

                for (int j = i + 1; j < n; j++)
                {
                    double covariance = stockDataList[i].StockRisk * stockDataList[j].StockRisk;
                    covarianceMatrix[i, j] = covariance;
                    covarianceMatrix[j, i] = covariance;
                }
            }

            return covarianceMatrix;
        }

        private static double[] MarkowitzModernization(double[,] covarianceMatrix)
        {
            int n = covarianceMatrix.GetLength(0);
            double[] weights = new double[n];

            // Расчет весов акций с разными значениями
            for (int i = 0; i < n; i++)
            {
                weights[i] = (i + 1) / (double)n;
            }

            return weights;
        }
    }
}
