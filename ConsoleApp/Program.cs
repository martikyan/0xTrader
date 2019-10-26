using _0xTrader.Core;
using _0xTrader.Core.Models;
using _0xTrader.Core.Services.Abstractions;
using Castle.Windsor;
using System;
using System.Threading.Tasks;

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
                bl.OnTrade += Bl_OnTrade;
                await bl.StartListeningAsync();
            }
        }

        private static void Bl_OnTrade(object sender, OnTradeEventArgs e)
        {
            Console.WriteLine("Trader Found!");
        }
    }
}