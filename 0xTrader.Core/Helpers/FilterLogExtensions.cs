using _0xTrader.Core.Models;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace _0xTrader.Core.Helpers
{
    public static class FilterLogExtensions
    {
        public static Fill ToFillModel(this FilterLog filterLog)
        {
            return new Fill()
            {
                //Buyer = filterLog,
            };
        }

        public static bool IsTradingFilterLog(this FilterLog filterLog)
        {
            if (filterLog.Topics.Length > 3)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
