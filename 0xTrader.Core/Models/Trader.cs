using System.Collections.Generic;

namespace _0xTrader.Core.Models
{
    public class Trader
    {
        public string Address { get; set; }
        public List<Wallet> Wallets { get; set; }
    }
}