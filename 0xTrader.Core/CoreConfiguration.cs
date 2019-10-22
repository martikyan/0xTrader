namespace _0xTrader.Core
{
    public class CoreConfiguration
    {
        public string NodeUrl { get; set; } = "http://localhost:8545";
        public int BlockScanThresholdSeconds { get; set; } = 1;
    }
}