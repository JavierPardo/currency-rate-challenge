using Database.Interfaces;
using Microsoft.Extensions.Configuration;
using Model;
using Model.DTO;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Database
{
    public class ExchangeRateSourceBRL : ExchangeRateSourceUSD, IExchangeRateSource
    {
        private readonly string _currenctCode = "BRL";
        private readonly CurrencyConfig _currentConfig = null;

        public ExchangeRateSourceBRL(IConfiguration configuration) : base(configuration)
        {
            var currencyConfigs = configuration.GetSection("CurrenciesConfig").Get<CurrencyConfig[]>();
            _currentConfig = currencyConfigs.FirstOrDefault(cc => cc.CurrencyCode == _currenctCode);

        }
        public new async Task<ExchangeRate> GetRate(CancellationToken cancelToken = default(CancellationToken))
        {
            var exchangeRate = await base.GetRate();
            exchangeRate.CurrencyCode = CurrencyCodeEnum.BRL.ToString();
            exchangeRate.Buy = exchangeRate.Buy / 4;
            exchangeRate.Sell = exchangeRate.Sell / 4;
            return exchangeRate;
        }
    }
}
