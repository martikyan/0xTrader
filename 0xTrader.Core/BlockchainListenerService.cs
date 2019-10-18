using System;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Web3;

namespace _0xTrader.Core
{
    public class BlockchainListenerService : IBlockchainListenerService
    {
        private readonly IWeb3 _web3;
        private long _lastReadBlock;

        public event EventHandler<UserTradedEventArgs> OnUserTraded;

        public BlockchainListenerService(IWeb3 web3)
        {
            _web3 = web3 ?? throw new ArgumentNullException(nameof(web3));
        }

        public async Task StartListeningAsync()
        {
            if (_lastReadBlock == default)
            {
                var pendingTxFilterId = await _web3.Eth.Filters.NewPendingTransactionFilter.SendRequestAsync();
                
                var changes = await _web3.Eth.Filters.GetFilterChangesForEthNewFilter.SendRequestAsync(pendingTxFilterId);
                var changes2 = await _web3.Eth.Filters.GetFilterChangesForBlockOrTransaction.SendRequestAsync(pendingTxFilterId);
            }
        }

        public Task StopListeningAsync()
        {
            _lastReadBlock = default;

            return Task.CompletedTask;
        }
    }
}