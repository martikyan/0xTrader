using _0xTrader.Core.Models;
using System.Numerics;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services.Abstractions
{
    public interface IBlockchainAccessor
    {
        Task<bool> IsSmartContractAsync(string address);

        Task<BigInteger> GetBalanceAsync(string tokenAddress, string ownerAddress);

        Task<(bool, Token)> TryGetTokenAsync(string address);

        Task<string> GetNameAsync(string tokenAddress);

        Task<string> GetSymbolAsync(string tokenAddress);

        Task<BigInteger> GetDecimalsAsync(string tokenAddress);
    }
}