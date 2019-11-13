using _0xTrader.Core.Models;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services.Abstractions
{
    public interface IRepoAccessor
    {
        void RegisterToken(Token token);
        void RegisterTrade(Trade trade);
        Token GetToken(string tokenAddress);
        Trade[] GetAllTrades(string traderAddress);
    }
}