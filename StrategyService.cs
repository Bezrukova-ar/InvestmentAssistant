using InvestmentAssistant.Model.Strategy;
using System;
using System.Collections.Generic;

namespace InvestmentAssistant
{
    /// <summary> Класс StrategyService предназначен для работы с данными для расчетов</summary>
    class StrategyService
    {      
        /// <summary> Прогнозирование доходности акции методом экспоненциального сглаживания</summary>
        public void ExponentialSmoothingProfitability(List<StockData> stockDataList, double[] data, double alpha, string securityId, string boardId)
        {
            double forecast = 0;

            // Инициализируем прогноз первым значением
            forecast = data[0];

            // Применяем экспоненциальное сглаживание для остальных значений
            for (int i = 0; i < data.Length; i++)
            {
                forecast = alpha * data[i] + (1 - alpha) * forecast;
                // Поиск индекса элемента в stockDataList с совпадающим SecurityId и BoardID                
                int index = stockDataList.FindIndex(x => x.SecurityId == securityId && x.BoardID == boardId);
                if (index != -1)
                    stockDataList[index].ProjectedStockReturn = forecast;
                else /*что-то додумать, добавлять новые записи, а еще обрабатывать случаи когда рассчитывается не число*/;
            }            
        }

        /// <summary> Прогнозирование рисков акции методом экспоненциального сглаживания</summary>
        public void ExponentialSmoothingRisk(List<StockData> stockDataList, double[] data, double alpha, string securityId, string boardId)
        {
            double forecast = 0;

            // Инициализируем прогноз первым значением
            forecast = data[0];

            // Применяем экспоненциальное сглаживание для остальных значений
            for (int i = 1; i < data.Length; i++)
            {
                forecast = alpha * data[i] + (1 - alpha) * forecast;
                // Поиск индекса элемента в stockDataList с совпадающим SecurityId и BoardID
                int index = stockDataList.FindIndex(x => x.SecurityId == securityId && x.BoardID == boardId);
                if (index != -1)
                    stockDataList[index].StockRisk = forecast;
                else /*что-то додумать, добавлять новые записи, а еще обрабатывать случаи когда рассчитывается не число*/;
            }         
        }

        /// <summary> Вычисление оптимального значения коэффициента сглаживания. Методом наименьших квадратов</summary>
        public double AutoExponentialSmoothing(double[] data)
        {
            double bestAlpha = 0.0;
            double bestMSE = double.MaxValue;

            for (double alpha = 0.1; alpha < 1.0; alpha += 0.1)
            {
                double forecast = 0;
                double mse = 0;

                forecast = data[0];

                for (int i = 1; i < data.Length; i++)
                {
                    forecast = alpha * data[i] + (1 - alpha) * forecast;
                    mse += Math.Pow(data[i] - forecast, 2);
                }

                mse /= data.Length;

                if (mse < bestMSE)
                {
                    bestMSE = mse;
                    bestAlpha = alpha;
                }
            }

            return bestAlpha;
        }      
    }
}
