using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentAssistant.Model.Chart
{
    // Создание модели данных для графика (не используется там где апи суванье данных)
    public class GainerData
    {
        public string SecurityName { get; set; }
        public double PercentageChange { get; set; }
        public int Index { get; set; }
    }
}
