using System;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services.Abstractions
{
    public interface IBlockchainListener
    {
        event EventHandler<UserTradedEventArgs> OnTrade;
        Task StartListeningAsync();
        Task StopListeningAsync();
    }
}