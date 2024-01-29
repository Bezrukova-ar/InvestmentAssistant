﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentAssistant
{
    /// <summary> Представляет экземпляр ценной бумаги с уникальным кодом и названием </summary>
    public class NameOfSecurities
    {  
        /// <summary> Уникальный код ценной бумаги </summary>
        public string SecurityId { get; set; }
        /// <summary> Название ценной бумаги </summary>
        public string SecurityName { get; set; }
    }

    /// <summary> Представляет структуру данных, используемую для хранения информации о финансовых свечах </summary>
    public class CandlestickData
    {
        /// <summary> Представляет цену открытия </summary>
        public decimal Open { get; set; }
        /// <summary> Представляет самую низкую цену </summary>
        public decimal Low { get; set; }
        /// <summary> Представляет самую высокую цену </summary>
        public decimal High { get; set; }
        /// <summary> Представляет цену закрытия  </summary>
        public decimal Close { get; set; }
        /// <summary> Представляет начальную дату и время периода времени, к которому относятся данные свечи </summary>
        public DateTime StartDate { get; set; }
        /// <summary> Представляет конечную дату и время периода времени, к которому относятся данные свечи </summary>
        public DateTime EndDate { get; set; }
    }

    /// <summary> Представляет структуру данных, используемую для хранения информации об истории торгов ценной бумаги </summary>
    public class SecurityTradingHistory
    { 
        /// <summary> Представляет режим торгов </summary>
        public string BoardID { get; set; }
        /// <summary> Представляет дату торгов </summary>
        public DateTime TradeDate { get; set; }
        /// <summary> Представляет номер торгов </summary>
        public int NumTrade { get; set; }
        /// <summary> Представляет значение торгов </summary>
        public double Volume { get; set; }
    }
}
