using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentAssistant.Model.Strategy
{
    /// <summary> Структура данных инвестиционного портфеля</summary>
    public class InvestmentPortfolio
    {
        public string SecurityId { get; set; }
        public string SecurityName { get; set; }
        public double Quantity { get; set; }
        public double TotalInvestment { get; set; }
    }
}
