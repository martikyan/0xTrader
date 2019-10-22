﻿using _0xTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _0xTrader.Core.Services
{
    public interface IRepoAccessor
    {
        Task RegisterTokenAsync(Token token);
        Task RegisterTokenAsync(string tokenAddress);
        Task<Token> GetTokenAsync(string tokenAddress);
    }
}
