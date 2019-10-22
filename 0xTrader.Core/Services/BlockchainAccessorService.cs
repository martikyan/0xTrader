using _0xTrader.Core.Helpers;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services
{
    public class BlockchainAccessorService : IBlockchainAccessor
    {
        private readonly IWeb3 _web3;

        public BlockchainAccessorService(IWeb3 web3)
        {
            _web3 = web3 ?? throw new ArgumentNullException(nameof(web3));
        }

        public async Task<BigInteger> GetBalanceAsync(string tokenAddress, string ownerAddress)
        {
            var contract = _web3.Eth.GetContract(Constants.ERC20.Abi, tokenAddress);
            var balanceFunc = contract.GetFunction(Constants.ERC20.BalanceOfFunction);

            return await balanceFunc.CallAsync<BigInteger>(ownerAddress);
        }

        public async Task<int> GetDecimalsAsync(string tokenAddress)
        {
            var contract = _web3.Eth.GetContract(Constants.ERC20.Abi, tokenAddress);
            var balanceFunc = contract.GetFunction(Constants.ERC20.DecimalsFunction);

            return await balanceFunc.CallAsync<int>();
        }

        public async Task<string> GetNameAsync(string tokenAddress)
        {
            var contract = _web3.Eth.GetContract(Constants.ERC20.Abi, tokenAddress);
            var balanceFunc = contract.GetFunction(Constants.ERC20.NameFunction);

            return await balanceFunc.CallAsync<string>();
        }

        public async Task<string> GetSymbolAsync(string tokenAddress)
        {
            var contract = _web3.Eth.GetContract(Constants.ERC20.Abi, tokenAddress);
            var balanceFunc = contract.GetFunction(Constants.ERC20.SymbolFunction);

            return await balanceFunc.CallAsync<string>();
        }

        public async Task<bool> IsERC20Async(string address)
        {
            var name = await GetNameAsync(address);
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return true;
        }
    }
}
