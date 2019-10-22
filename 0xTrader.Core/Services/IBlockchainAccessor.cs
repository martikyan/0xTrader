﻿using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services
{
    public interface IBlockchainAccessor
    {
        Task<BigInteger> GetBalanceAsync(string tokenAddress, string ownerAddress);

        Task<bool> IsERC20Async(string address);

        Task<string> GetNameAsync(string tokenAddress);

        Task<string> GetSymbolAsync(string tokenAddress);

        Task<int> GetDecimalsAsync(string tokenAddress);
    }
}