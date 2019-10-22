using System;
using _0xTrader.Core;
using _0xTrader.Core.Services;
using Nethereum.Web3;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var web3 = new Web3();
            var config = new CoreConfiguration();
            var bls = new BlockchainListenerService(config, web3);
            bls.StartListeningAsync().Wait();

            //var ba = new BlockchainAccessorService(web3);
            //var repo = new RepoAccessorService(ba);
            //repo.RegisterTokenAsync("0xdAC17F958D2ee523a2206206994597C13D831ec7").Wait();
        }
    }
}
