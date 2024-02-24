using InvestmentAssistant.Model.Strategy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace InvestmentAssistant
{
    class PortfolioOptimization
    {
        /// <summary> Оптимизация портфеля по методу Шарпа</summary>
        public List<PortfolioAsset> OptimizePortfolio(List<PortfolioAsset> portfolioList, List<StockData> stockDataList)
        {
            /* stockDataList.RemoveAll(x => double.IsNaN(x.ProjectedStockReturn) || double.IsNaN(x.StockRisk));

             // Рассчитать вес для каждого актива
             double totalInvestment = portfolioList.Sum(asset => asset.TotalInvestment);
             foreach (var asset in portfolioList)
             {
                 asset.Weight = asset.TotalInvestment / totalInvestment;
             }

             List<PortfolioAsset> newPortfolioList = new List<PortfolioAsset>();

             // Перераспределение активов на основе весов
             foreach (var asset in portfolioList)
             {
                 // Рассчитываем новое количество на основе веса
                 double newTotalInvestment = totalInvestment * asset.Weight;
                 double pricePerShare = stockDataList.First(stock => stock.SecurityId == asset.SecurityId).CurrentSharePrice;
                 int newQuantity = (int)(newTotalInvestment / pricePerShare);

                 // Создать новый объект PortfolioAsset с обновленными данными
                 PortfolioAsset newAsset = new PortfolioAsset
                 {
                     SecurityId = asset.SecurityId,
                     SecurityName = asset.SecurityName,
                     BoardID = asset.BoardID,
                     Quantity = newQuantity,
                     TotalInvestment = newQuantity * pricePerShare,
                     Weight = asset.Weight,
                     StockRisk = stockDataList.First(stock => stock.SecurityId == asset.SecurityId).StockRisk
                 };

                 newPortfolioList.Add(newAsset);
             }

             return newPortfolioList;*/

            // Проверка наличия данных по акциям из портфеля в stockDataList
            if (portfolioList.Any(asset => !stockDataList.Any(stock => stock.SecurityId == asset.SecurityId)))
            {
                // Если хотя бы одной акции нет в stockDataList, прерываем метод
                return null;
            }

            // Проверка на отрицательные значения Quantity и TotalInvestment
            if (portfolioList.Any(asset => asset.Quantity < 0 || asset.TotalInvestment < 0))
            {
                // Если Quantity или TotalInvestment отрицательные, прерываем метод
                return null;
            }

            stockDataList.RemoveAll(x => double.IsNaN(x.ProjectedStockReturn) || double.IsNaN(x.StockRisk));

            // Рассчитать вес для каждого актива
            double totalInvestment = portfolioList.Sum(asset => asset.TotalInvestment);
            foreach (var asset in portfolioList)
            {
                asset.Weight = asset.TotalInvestment / totalInvestment;
            }

            List<PortfolioAsset> newPortfolioList = new List<PortfolioAsset>();

            // Перераспределение активов на основе весов
            foreach (var asset in portfolioList)
            {
                // Рассчитываем новое количество на основе веса
                double newTotalInvestment = totalInvestment * asset.Weight;
                double pricePerShare = stockDataList.First(stock => stock.SecurityId == asset.SecurityId).CurrentSharePrice;
                int newQuantity = (int)(newTotalInvestment / pricePerShare);

                // Создать новый объект PortfolioAsset с обновленными данными
                PortfolioAsset newAsset = new PortfolioAsset
                {
                    SecurityId = asset.SecurityId,
                    SecurityName = asset.SecurityName,
                    BoardID = asset.BoardID,
                    Quantity = newQuantity,
                    TotalInvestment = newQuantity * pricePerShare,
                    Weight = asset.Weight,
                    StockRisk = stockDataList.First(stock => stock.SecurityId == asset.SecurityId).StockRisk
                };

                newPortfolioList.Add(newAsset);
            }

            return newPortfolioList;
        }
    }
}
