using _0xTrader.Core.Models;
using _0xTrader.Core.Services.Abstractions;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
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

        private long _lastScannedBlock = 8800724;
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
                await Task.Delay(TimeSpan.FromSeconds(_config.BlockScanThresholdSeconds));
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
            if (receipt.Status.Value.IsZero)
            {
                return;
            }

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
                        var trade = await ConstructTradeFromEvents(eo, ei, receipt);
                        if (trade == null)
                        {
                            continue;
                        }

                        var _1stTrader = trade.Token1Holder.Address;
                        var _2ndTrader = trade.Token2Holder.Address;
                        var _1stTraderIsContract = await _blockchainAccessor.IsSmartContractAsync(_1stTrader);
                        var _2ndTraderIsContract = await _blockchainAccessor.IsSmartContractAsync(_2ndTrader);

                        if (_2ndTraderIsContract && _2ndTraderIsContract)
                        {
                            continue;
                        }

                        OnTrade?.Invoke(this, new OnTradeEventArgs() { Trade = trade });
                    }
                }
            }
        }

        private async Task<Trade> ConstructTradeFromEvents(EventLog<TransferEvent> eventLog1, EventLog<TransferEvent> eventLog2, TransactionReceipt receipt)
        {
            var (success1, token1) = await _blockchainAccessor.TryGetTokenAsync(eventLog1.Log.Address);
            var (success2, token2) = await _blockchainAccessor.TryGetTokenAsync(eventLog2.Log.Address);

            if (!success1 || !success2 ||
                string.Equals(token1.Symbol, token2.Symbol))
            {
                return null;
            }

            var token1Wallet = new Wallet()
            {
                Token = token1,
                Balance = await _blockchainAccessor.GetBalanceAsync(eventLog1.Log.Address, eventLog1.Event.To),
            };

            var token2Wallet = new Wallet()
            {
                Token = token2,
                Balance = await _blockchainAccessor.GetBalanceAsync(eventLog2.Log.Address, eventLog2.Event.To),
            };

            var token1Holder = new Trader()
            {
                Address = eventLog1.Event.To,
                Wallets = new List<Wallet>() { token1Wallet },
            };

            var token2Holder = new Trader()
            {
                Address = eventLog2.Event.To,
                Wallets = new List<Wallet>() { token2Wallet },
            };

            var trade = new Trade()
            {
                Token1 = token1,
                Token2 = token2,
                Token1Holder = token1Holder,
                Token2Holder = token2Holder,
                TransactionHash = receipt.TransactionHash,
                TradeBlockNumber = (long)receipt.BlockNumber.Value,
            };

            return trade;
        }
    }
}