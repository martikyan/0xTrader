using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0xTrader.Core.Models;
using _0xTrader.Core.Services.Abstractions;

namespace _0xTrader.Core.Services
{
    public class ProfitCalculatorService : IProfitCalculator
    {
        private readonly CoreConfiguration _config;
        private readonly IRepoAccessor _repoAccessor;

        public ProfitCalculatorService(CoreConfiguration config, IRepoAccessor repoAccessor)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _repoAccessor = repoAccessor ?? throw new ArgumentNullException(nameof(repoAccessor));
        }

        public ProfitReport CalculateTradesProfit(string traderAddress)
        {
            var result = new ProfitReport()
            {
                TraderAddress = traderAddress,
            };

            var filteredTrades = _repoAccessor
                .GetAllTrades(traderAddress)
                .Where(t =>
                    _repoAccessor.GetToken(t.Token1.Address) != null &&
                    _repoAccessor.GetToken(t.Token2.Address) != null)
                .OrderBy(t => t.TradeBlockNumber)
                .ToArray();

            if (filteredTrades.Count <>)
        }
    }
}