using _0xTrader.Core.Helpers;
using _0xTrader.Core.Models;
using _0xTrader.Core.Services.Abstractions;
using Nethereum.Web3;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services
{
    public class BlockchainAccessorService : IBlockchainAccessor
    {
        private readonly IWeb3 _web3;
        private readonly CoreConfiguration _config;

        public BlockchainAccessorService(IWeb3 web3, CoreConfiguration configuration)
        {
            _web3 = web3 ?? throw new ArgumentNullException(nameof(web3));
            _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<BigInteger> GetBalanceAsync(string tokenAddress, string ownerAddress)
        {
            var contract = _web3.Eth.GetContract(Constants.ERC20.Abi, tokenAddress);
            var balanceFunc = contract.GetFunction(Constants.ERC20.BalanceOfFunction);

            return await balanceFunc.CallAsync<BigInteger>(ownerAddress);
        }

        public async Task<BigInteger> GetDecimalsAsync(string tokenAddress)
        {
            var contract = _web3.Eth.GetContract(Constants.ERC20.Abi, tokenAddress);
            var decimalFunc = contract.GetFunction(Constants.ERC20.DecimalsFunction);
            var decimals = await decimalFunc.CallAsync<BigInteger>();

            return decimals;
        }

        public async Task<string> GetNameAsync(string tokenAddress)
        {
            var contract = _web3.Eth.GetContract(Constants.ERC20.Abi, tokenAddress);
            var nameFunc = contract.GetFunction(Constants.ERC20.NameFunction);
            var name = await nameFunc.CallAsync<string>();

            return name;
        }

        public async Task<string> GetSymbolAsync(string tokenAddress)
        {
            var contract = _web3.Eth.GetContract(Constants.ERC20.Abi, tokenAddress);
            var symbolFunc = contract.GetFunction(Constants.ERC20.SymbolFunction);
            var symbol = await symbolFunc.CallAsync<string>();

            return symbol;
        }

        public async Task<bool> IsSmartContractAsync(string address)
        {
            var code = await _web3.Eth.GetCode.SendRequestAsync(address);
            if (code == "0x")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<(bool, Token)> TryGetTokenAsync(string address)
        {
            try
            {
                var token = new Token()
                {
                    Address = address,
                    Name = await GetNameAsync(address),
                    Symbol = await GetSymbolAsync(address),
                    Decimals = (int)await GetDecimalsAsync(address),
                };

                if (string.IsNullOrWhiteSpace(token.Name) ||
                    string.IsNullOrWhiteSpace(token.Symbol) ||
                    token.Decimals > byte.MaxValue)
                {
                    return (false, null);
                }

                return (true, token);
            }
            catch (Exception e) // TODO remove exc
            {
                return (false, null);
            }
        }
    }
}