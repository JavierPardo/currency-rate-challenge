using Database.Interfaces;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;
using Model.DTO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Database
{
    public class ExchangeRateSourceUSD : IExchangeRateSource
    {
        private readonly string _currenctCode = "USD";
        private readonly CurrencyConfig _currentConfig = null;
        protected readonly ILogger<ExchangeRateSourceUSD> _logger;

        public ExchangeRateSourceUSD(IConfiguration configuration, ILogger<ExchangeRateSourceUSD> logger)
        {
            var currencyConfigs = configuration.GetSection("CurrenciesConfig").Get<CurrencyConfig[]>();
            _currentConfig = currencyConfigs.FirstOrDefault(cc => cc.CurrencyCode == _currenctCode);
            _logger = logger;
        }

        public decimal GetLimit()
        {
            _logger.LogInformation($"Monthy limit :{_currentConfig.Limit} --- For:{_currentConfig.CurrencyCode}");
            return _currentConfig.Limit;
        }
        public async Task<ExchangeRate> GetRate(CancellationToken cancelToken = default(CancellationToken))
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_currentConfig.Uri, cancelToken);
                var currencyResponse = await response.Content.ReadAsAsync<JArray>();

                _logger.LogInformation($"Information from server: {currencyResponse.ToString()}");
                
                return new ExchangeRate
                {
                    Sell = currencyResponse[0].Value<decimal>(),
                    Buy = currencyResponse[1].Value<decimal>(),
                    CurrencyCode = _currenctCode
                };
            }
        }
    }
}
