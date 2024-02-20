﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentAssistant.Model
{    /// <summary> Представляет структуру данных, используемую для сопоставления стратегии и ее условий </summary>
    class InvestmentStrategiesAndTheirConditions
    {
        /// <summary> Название стратегии</summary>
        public string NameStrategies { get; set; }
        /// <summary> Цель инвестирования</summary>
        public string InvestmentGoal { get; set; }
        /// <summary> Инвестиционный горизонт</summary>
        public string InvestmentHorizon { get; set; }
        /// <summary> Учёт рискоа</summary>
        public string RiskAccounting { get; set; }
        /// <summary> Ожидаемая доходность</summary>
        public string ExpectedReturn { get; set; }
    }
}
