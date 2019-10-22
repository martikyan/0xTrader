using System;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services
{
    public interface IBlockchainListener
    {
        event EventHandler<UserTradedEventArgs> OnUserTraded;
        Task StartListeningAsync();
        Task StopListeningAsync();
    }
}