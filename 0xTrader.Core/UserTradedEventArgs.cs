using _0xTrader.Core.Models;
using System;

namespace _0xTrader.Core
{
    public class UserTradedEventArgs : EventArgs
    {
        public Trade Trade { get; set; }

        public UserTradedEventArgs()
            : base()
        {
        }
    }
}