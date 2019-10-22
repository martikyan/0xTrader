using System;
using System.Collections.Generic;
using System.Text;

namespace _0xTrader.Core.Models
{
    public class Fill
    {
        public string Currency1 { get; set; }
        public string Currency2 { get; set; }
        public Trader Buyer { get; set; }
        public Trader Seller { get; set; }
        public DateTimeOffset FillDate { get; set; }
    }
}
