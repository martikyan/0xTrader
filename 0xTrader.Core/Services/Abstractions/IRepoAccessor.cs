using _0xTrader.Core.Models;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services.Abstractions
{
    public interface IRepoAccessor
    {
        Task RegisterTokenAsync(Token token);

        Task RegisterTokenAsync(string tokenAddress);

        Task<Token> GetTokenAsync(string tokenAddress);
    }
}