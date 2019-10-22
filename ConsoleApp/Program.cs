using System;
using System.Threading;
using System.Threading.Tasks;
using _0xTrader.Core;
using _0xTrader.Core.Services.Abstractions;
using Castle.Windsor;
using Nethereum.Web3;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        public static async Task MainAsync(string[] args)
        {
            using (var container = new WindsorContainer())
            {
                container.Install(new CoreInstaller());
                var bl = container.Resolve<IBlockchainListener>();
                bl.OnTrade += Bl_OnUserTraded;
                await bl.StartListeningAsync();
            }
        }

        private static void Bl_OnUserTraded(object sender, UserTradedEventArgs e)
        {
            Console.WriteLine("Trader Found!");
        }
    }
}
