namespace _0xTrader.Core.Models
{
    public class Trade
    {
        public Token Token1 { get; set; }
        public Token Token2 { get; set; }
        public Trader Token1Holder { get; set; }
        public Trader Token2Holder { get; set; }
    }
}