using System;
using System.Collections.Generic;
using System.Text;

namespace _0xTrader.Core.Models
{
    public class TradeNode
    {
        public double Price { get; set; }
        public string TraderAddress { get; set; }
        public Token BuyingToken { get; set; }
        public Token SellingToken { get; set; }

    }
}
