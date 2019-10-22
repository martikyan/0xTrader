using System;
using System.Linq;
using System.Threading.Tasks;
using _0xTrader.Core.Helpers;
using _0xTrader.Core.Models;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Web3;
using Nethereum.Util;
using Nethereum.Web3;

namespace _0xTrader.Core.Services
{
    public class BlockchainListenerService : IBlockchainListener
    {
        private readonly CoreConfiguration _config;
        private readonly IWeb3 _web3;
        public event EventHandler<UserTradedEventArgs> OnUserTraded;

        public BlockchainListenerService(CoreConfiguration config, IWeb3 web3)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _web3 = web3 ?? throw new ArgumentNullException(nameof(web3));
        }

        public async Task StartListeningAsync()
        {
            var sha3 = new Sha3Keccack();

            var fi = new NewFilterInput()
            {
                FromBlock = BlockParameter.CreateLatest(),
                ToBlock = BlockParameter.CreatePending(),
                //Address = _config.DexAddresses,
            };
            var eventHandler = _web3.Eth.GetEvent<TransferEventDTO>();
            var eventFilter = await eventHandler.CreateFilterAsync(fi);

            while (true)
            {
                var logs = await eventHandler.GetAllChanges(eventFilter);
                if (logs.Count != 0)
                {
                    Console.WriteLine("Debug me");
                }

                await Task.Delay(10000);
            }

        }

        public Task StopListeningAsync()
        {
            return Task.CompletedTask;
        }
    }
}