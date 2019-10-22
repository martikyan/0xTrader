using _0xTrader.Core.Models;
using _0xTrader.Core.Services.Abstractions;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services
{
    public class BlockchainListenerService : IBlockchainListener
    {
        private readonly CoreConfiguration _config;
        private readonly IBlockchainAccessor _blockchainAccessor;
        private readonly IWeb3 _web3;

        private long _lastScannedBlock = 5106317;
        private bool _isStarted = false;

        public event EventHandler<OnTradeEventArgs> OnTrade;

        public BlockchainListenerService(CoreConfiguration config, IBlockchainAccessor blockchainAccessor, IWeb3 web3)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _blockchainAccessor = blockchainAccessor ?? throw new ArgumentNullException(nameof(blockchainAccessor));
            _web3 = web3 ?? throw new ArgumentNullException(nameof(web3));
        }

        public async Task StartListeningAsync()
        {
            if (_isStarted)
            {
                throw new InvalidOperationException($"The {nameof(BlockchainListenerService)} was already started.");
            }

            _isStarted = true;
            while (true)
            {
                await RetreiveUnscannedBlocks();
            }
        }

        public async Task RetreiveUnscannedBlocks()
        {
            var lastBlockNumber = (long)(await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).Value;

            if (_lastScannedBlock >= lastBlockNumber)
            {
                await Task.Delay(1000);
                return;
            }

            var blockToScan = Math.Min(_lastScannedBlock + 1, lastBlockNumber);
            if (blockToScan <= 1)
            {
                blockToScan = lastBlockNumber;
            }

            for (var i = blockToScan; i < lastBlockNumber + 1; i++)
            {
                var block = await _web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(new BlockParameter(new HexBigInteger(i)));
                foreach (var txHash in block.TransactionHashes)
                {
                    await HandleTransactionEventsAsync(txHash);
                }

                Debug.Assert(i > _lastScannedBlock);
                _lastScannedBlock = i;
            }
        }

        public Task StopListeningAsync()
        {
            _isStarted = false;
            return Task.CompletedTask;
        }

        private async Task HandleTransactionEventsAsync(string txHash)
        {
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);
            var events = receipt.DecodeAllEvents<TransferEvent>();
            events = events.Where(e => e.Event.From != e.Event.To).ToList();

            if (events.Count < 2)
            {
                return;
            }

            foreach (var eo in events)
            {
                foreach (var ei in events)
                {
                    if (eo.Event.From == ei.Event.To &&
                        eo.Event.To == ei.Event.From &&
                        eo.Log.Address != ei.Log.Address)
                    {
                        var trade = await ConstructTradeFromEvents(eo, ei);
                        OnTrade?.Invoke(this, new OnTradeEventArgs() { Trade = trade });
                    }
                }
            }
        }

        private async Task<Trade> ConstructTradeFromEvents(EventLog<TransferEvent> eventLog1, EventLog<TransferEvent> eventLog2)
        {
            var token1 = await GetTokenByAddress(eventLog1.Log.Address);
            var token2 = await GetTokenByAddress(eventLog2.Log.Address);
            var token1Holder = new Trader()
            {
                Address = eventLog1.Event.To,
                Balance = await _blockchainAccessor.GetBalanceAsync(eventLog1.Log.Address, eventLog1.Event.To),
            };
            var token2Holder = new Trader()
            {
                Address = eventLog2.Event.To,
                Balance = await _blockchainAccessor.GetBalanceAsync(eventLog2.Log.Address, eventLog2.Event.To),
            };

            var trade = new Trade()
            {
                Token1 = token1,
                Token2 = token2,
                Token1Holder = token1Holder,
                Token2Holder = token2Holder,
            };

            return trade;
        }

        private async Task<Token> GetTokenByAddress(string tokenAddress)
        {
            var token = new Token()
            {
                Address = tokenAddress,
                Name = await _blockchainAccessor.GetNameAsync(tokenAddress),
                Symbol = await _blockchainAccessor.GetSymbolAsync(tokenAddress),
                Decimals = await _blockchainAccessor.GetDecimalsAsync(tokenAddress),
            };

            return token;
        }
    }
}