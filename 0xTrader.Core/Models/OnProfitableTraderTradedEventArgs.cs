using System;
using System.Collections.Generic;
using System.Text;

namespace _0xTrader.Core.Models
{
    public class OnProfitableTraderTradedEventArgs : EventArgs
    {
        public TraderProfit TraderProfit { get; set; } 
        public OnProfitableTraderTradedEventArgs()
        {
        }
    }
}
