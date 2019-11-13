using System;
using System.Collections.Generic;
using System.Text;

namespace _0xTrader.Core.Models
{
    public class ProfitReport
    {
        public Token ReportToken { get; set; }
        public bool IsFullReport { get; set; }
        public string TraderAddress { get; set; }
        public long TotalTradesCount { get; set; }
        public double ProfitPercentage { get; set; }
        public double SuccessFailureRatio { get; set; }
        public TimeSpan AverageTradeThreshold { get; set; }      
    }
}
