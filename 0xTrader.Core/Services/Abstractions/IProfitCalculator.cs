using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0xTrader.Core.Models;

namespace _0xTrader.Core.Services.Abstractions
{
    public interface IProfitCalculator
    {
        ProfitReport CalculateTradesProfit(string traderAddress);
    }
}
