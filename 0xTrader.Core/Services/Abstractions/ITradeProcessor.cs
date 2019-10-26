using _0xTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _0xTrader.Core.Services.Abstractions
{
    public interface ITradeProcessor
    {
        void ValidateTrade(Trade trade);
        void RegisterTrade(Trade trade);
        event EventHandler<OnProfitableTraderTradedEventArgs> OnProfitableTraderTraded;
    }
}
