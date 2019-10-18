using System;
using System.Threading.Tasks;

namespace _0xTrader.Core
{
    public interface IBlockchainListenerService
    {
        event EventHandler<UserTradedEventArgs> OnUserTraded;

        Task StartListeningAsync();
        Task StopListeningAsync();
    }
}