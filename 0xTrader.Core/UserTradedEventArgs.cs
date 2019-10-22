using _0xTrader.Core.Models;
using System;

namespace _0xTrader.Core
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