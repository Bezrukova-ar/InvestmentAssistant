using InvestmentAssistant.Model.Strategy;
using System.Collections.Generic;
using System.Linq;

namespace InvestmentAssistant
{
    public class PortfolioBuilder
    {
        /// <summary> Формирование инвестиционного портфеля консервативной стратегией</summary>
        public static List<InvestmentPortfolio> BuildConservativePortfolio(List<StockData> stockDataList, double capital)
        {
            stockDataList.RemoveAll(x => double.IsNaN(x.ProjectedStockReturn) || double.IsNaN(x.StockRisk));

            //зависит от стратегии
            stockDataList = stockDataList.OrderBy(x => x.StockRisk)
                                         .ThenByDescending(x => x.CurrentSharePrice)
                                         .ToList();

            // Создаем матрицу ковариации
            double[,] covarianceMatrix = CalculateCovarianceMatrix(stockDataList);

            // Определяем веса акций с помощью алгоритма Марковица
            double[] weights = MarkowitzAlgorithm(covarianceMatrix);

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
                        if (sum >= capital)
                            return portfolio;


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
            }

            return portfolio;
        }

        /// <summary> Расчет матрицы ковариантности </summary>
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

        /// <summary> Алгоритм макровица для консервативной стратегии </summary>
        private static double[] MarkowitzAlgorithm(double[,] covarianceMatrix)
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

        /// <summary> Формирование инвестиционного портфеля сбалансированной стратегией</summary>
        public static List<InvestmentPortfolio> BuildBalancedPortfolio(List<StockData> stockDataList, double capital)
        {
            stockDataList.RemoveAll(x => double.IsNaN(x.ProjectedStockReturn) || double.IsNaN(x.StockRisk));

            // Рассчитываем границу риска
            double totalRisk = stockDataList.Sum(x => x.StockRisk);
            double riskThreshold = totalRisk / stockDataList.Count;

            // Разделяем акции на низкоуровневые и высокоуровневые
            List<StockData> lowRiskStocks = stockDataList.Where(x => x.StockRisk <= riskThreshold).ToList();
            List<StockData> highRiskStocks = stockDataList.Where(x => x.StockRisk > riskThreshold).ToList();

            lowRiskStocks = lowRiskStocks.OrderByDescending(x => x.StockRisk)
                                         .ThenByDescending(x => x.CurrentSharePrice)
                                         .ToList();

            highRiskStocks = highRiskStocks.OrderBy(x => x.StockRisk)
                                           .ThenByDescending(x => x.CurrentSharePrice)
                                           .ToList();

            // Создаем инвестиционный портфель
            List<InvestmentPortfolio> portfolio = new List<InvestmentPortfolio>();
            double remainingCapital = capital;

            double investmentAmount = capital * 0.3; // 30% от капитала
            double sum = 0;

            // Первый цикл: выбираем низкоуровневые акции до тех пор, пока не израсходуется 30% капитала
            int i = 0;
            while (i < lowRiskStocks.Count && sum < investmentAmount)
            {
                StockData stock = lowRiskStocks[i];
                int quantity = (int)(investmentAmount / stock.CurrentSharePrice);

                if (quantity > 0 && stock.CurrentSharePrice < investmentAmount - sum)
                {
                    // Дополнительная проверка для уменьшения quantity, если необходимо
                    if (quantity * stock.CurrentSharePrice > investmentAmount - sum)
                    {
                        quantity = (int)((investmentAmount - sum) / stock.CurrentSharePrice);
                    }

                    sum += quantity * stock.CurrentSharePrice;
                    InvestmentPortfolio investment = new InvestmentPortfolio
                    {
                        SecurityId = stock.SecurityId,
                        SecurityName = stock.SecurityName,
                        BoardID = stock.BoardID,
                        Quantity = quantity,
                        TotalInvestment = quantity * stock.CurrentSharePrice
                    };
                    portfolio.Add(investment);
                }
                i++;
            }

            // Второй цикл: на оставшийся капитал выбираем высокоуровневые акции
            int j = 0;
            while (j < highRiskStocks.Count && sum < capital)
            {
                StockData stock = highRiskStocks[j];
                int quantity = (int)(remainingCapital / stock.CurrentSharePrice);

                if (quantity > 0 && stock.CurrentSharePrice < capital - sum)
                {
                    // Дополнительная проверка для уменьшения quantity, если необходимо
                    if (quantity * stock.CurrentSharePrice > capital - sum)
                    {
                        quantity = (int)((capital - sum) / stock.CurrentSharePrice);
                    }
                    sum += quantity * stock.CurrentSharePrice;
                    InvestmentPortfolio investment = new InvestmentPortfolio
                    {
                        SecurityId = stock.SecurityId,
                        SecurityName = stock.SecurityName,
                        BoardID = stock.BoardID,
                        Quantity = quantity,
                        TotalInvestment = quantity * stock.CurrentSharePrice
                    };
                    portfolio.Add(investment);
                    
                }
                j++;
            }

            return portfolio;         
        }

        /// <summary> Формирование инвестиционного портфеля агрессивной стратегией</summary>
        public static List<InvestmentPortfolio> BuildAggressivePortfolio(List<StockData> stockDataList, double capital)
        {     
            stockDataList.RemoveAll(x => double.IsNaN(x.ProjectedStockReturn) || double.IsNaN(x.StockRisk));

            stockDataList = stockDataList.OrderByDescending(x => x.CurrentSharePrice) 
                                         .ThenBy(x => x.StockRisk).ThenBy(x => x.ProjectedStockReturn)
                                         .ToList();

            // Создаем матрицу ковариации
            double[,] covarianceMatrix = CalculateCovarianceMatrix(stockDataList);

            // Определяем веса акций с помощью алгоритма Марковица
            double[] weights = MarkowitzAlgorithm(covarianceMatrix);

            // Создаем инвестиционный портфель
            List<InvestmentPortfolio> portfolio = new List<InvestmentPortfolio>();
            double sum = 0;
            for (int i = 0; i < stockDataList.Count; i++)
            {
                double investmentAmount = capital * weights[i];

                if (investmentAmount > 0)
                {
                    int quantity = (int)(investmentAmount / stockDataList[i].CurrentSharePrice);

                    if (sum >= capital)
                        return portfolio;
                    // Дополнительная проверка для уменьшения quantity, если необходимо
                    if (quantity * stockDataList[i].CurrentSharePrice > capital - sum)
                    {
                        quantity = (int)((capital - sum) / stockDataList[i].CurrentSharePrice);
                    }
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
            }

            return portfolio;
        }

        /// <summary> Формирование инвестиционного портфеля пассивной стратегией</summary>
        public static List<InvestmentPortfolio> BuildPassivePortfolio(List<StockData> stockDataList, double capital)
        {
            stockDataList.RemoveAll(x => double.IsNaN(x.ProjectedStockReturn) || double.IsNaN(x.StockRisk));

            stockDataList = stockDataList.OrderByDescending(x => x.CurrentSharePrice)
                                         .ToList();

            // Создаем матрицу ковариации
            double[,] covarianceMatrix = CalculateCovarianceMatrix(stockDataList);

            // Определяем веса акций с помощью алгоритма Марковица
            double[] weights = MarkowitzAlgorithm(covarianceMatrix);

            // Создаем инвестиционный портфель
            List<InvestmentPortfolio> portfolio = new List<InvestmentPortfolio>();
            double sum = 0;
            for (int i = 0; i < stockDataList.Count; i++)
            {
                double investmentAmount = capital * weights[i];

                if (investmentAmount > 0 )
                {
                    int quantity = (int)(investmentAmount / stockDataList[i].CurrentSharePrice);

                    if (quantity > 0 && stockDataList[i].CurrentSharePrice < capital - sum)
                    {

                        if (sum >= capital)
                            return portfolio;



                        // Дополнительная проверка для уменьшения quantity, если необходимо
                        if (quantity * stockDataList[i].CurrentSharePrice > capital - sum)
                        {
                            quantity = (int)((capital - sum) / stockDataList[i].CurrentSharePrice);
                        }
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
            }

            return portfolio;
        }   

        /// <summary> Формирование инвестиционного портфеля активной стратегией</summary>
        public static List<InvestmentPortfolio> BuildActivePortfolio(List<StockData> stockDataList, double capital)
        {
            stockDataList.RemoveAll(x => double.IsNaN(x.ProjectedStockReturn) || double.IsNaN(x.StockRisk));

            // Зависит от стратегии
            stockDataList = stockDataList.OrderBy(x => x.ProjectedStockReturn)
                                         .ThenByDescending(x => x.StockRisk)
                                         .ThenBy(x => x.CurrentSharePrice)
                                         .ToList();

            // Создаем матрицу ковариации
            double[,] covarianceMatrix = CalculateCovarianceMatrix(stockDataList);

            // Определяем веса акций с помощью алгоритма Марковица для активной стратегии
            double[] weights = MarkowitzAlgorithmForActiveStrategy(stockDataList, covarianceMatrix);

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
                        if (sum >= capital)
                            return portfolio;

                        // Дополнительная проверка для уменьшения quantity, если необходимо
                        if (quantity * stockDataList[i].CurrentSharePrice > investmentAmount - sum)
                        {
                            quantity = (int)((investmentAmount - sum) / stockDataList[i].CurrentSharePrice);
                        }

                        // Добавляем акцию в портфель только если количество больше 0
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
                }
            }

            return portfolio;
        }

        /// <summary> Алгоритм Марковица для активной стратегии </summary>
        private static double[] MarkowitzAlgorithmForActiveStrategy(List<StockData> stockDataList, double[,] covarianceMatrix)
        {
            int n = stockDataList.Count;
            double[] weights = new double[n];

            // Расчет весов акций на основе цены, доходности и рисков
            double totalRisk = stockDataList.Sum(x => x.StockRisk);
            for (int i = 0; i < n; i++)
            {
                // Вес пропорционален обратному риску, чем ниже риск, тем выше вес
                weights[i] = (totalRisk - stockDataList[i].StockRisk) / totalRisk;
            }

            return weights;
        }
    }
}
