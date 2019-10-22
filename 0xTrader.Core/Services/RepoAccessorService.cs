using _0xTrader.Core.Models;
using _0xTrader.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services
{
    public class RepoAccessorService : IRepoAccessor
    {
        private readonly IBlockchainAccessor _blockchainAccessor;

        public RepoAccessorService(IBlockchainAccessor blockchainAccessor)
        {
            _blockchainAccessor = blockchainAccessor ?? throw new ArgumentNullException(nameof(blockchainAccessor));
        }

        public Task<Token> GetTokenAsync(string tokenAddress)
        {
            return Task.FromResult(Repo.Tokens[tokenAddress.ToLower()]);
        }

        public Task RegisterTokenAsync(Token token)
        {
            token.Address = token.Address.ToLower();
            Repo.Tokens[token.Address] = token;
            return Task.CompletedTask;
        }

        public async Task RegisterTokenAsync(string tokenAddress)
        {
            var token = new Token()
            {
                Address = tokenAddress,
                Name = await _blockchainAccessor.GetNameAsync(tokenAddress),
                Symbol = await _blockchainAccessor.GetSymbolAsync(tokenAddress),
                Decimals = await _blockchainAccessor.GetDecimalsAsync(tokenAddress),
            };

            await RegisterTokenAsync(token);
        }

        private static class Repo
        {
            public static IDictionary<string, Token> Tokens { get; set; } = new Dictionary<string, Token>();
        }
    }
}