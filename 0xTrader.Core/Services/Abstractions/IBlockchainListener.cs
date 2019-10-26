using _0xTrader.Core.Models;
using System;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services.Abstractions
{
    public interface IBlockchainListener
    {
        event EventHandler<OnTradeEventArgs> OnTrade;

        Task StartListeningAsync();

        Task StopListeningAsync();
    }
}