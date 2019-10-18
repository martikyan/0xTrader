using System;
using _0xTrader.Core;
using Nethereum.Web3;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var web3 = new Web3();
            var bls = new BlockchainListenerService(web3);
            bls.StartListeningAsync().Wait();
        }
    }
}
