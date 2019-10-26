using System;

namespace _0xTrader.Core.Models
{
    public class OnTradeEventArgs : EventArgs
    {
        public Trade Trade { get; set; }

        public OnTradeEventArgs()
            : base()
        {
        }
    }
}