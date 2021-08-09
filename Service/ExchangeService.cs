using Service.Interfaces;
using Database.Interfaces;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Service
{
    public class ExchangeService: IExchangeService
    {
        private Func<CurrencyCodeEnum, IExchangeRateSource> _exchangeSourceSolver;
        private readonly ILogger<ExchangeService> _logger;

        public ExchangeService(Func<CurrencyCodeEnum, IExchangeRateSource> exchangeSourceSolver, ILogger<ExchangeService> logger )
        {
            _exchangeSourceSolver = exchangeSourceSolver;
            _logger = logger;
        }

        public async IAsyncEnumerable<ExchangeRate> GetAllRates()
        {
            foreach (CurrencyCodeEnum currencyCode in (CurrencyCodeEnum[])Enum.GetValues(typeof(CurrencyCodeEnum)))
            {
                if (currencyCode < 0)
                    continue;
                yield return await GetRateByCurrencyCode(currencyCode.ToString());
            }
            yield break;
        }

        private IExchangeRateSource GetExchangeSourceBySourceCode(string currencyCode)
        {
            _logger.LogInformation($"Getting Exchange Source for: {currencyCode}");
            object currentCurrencyCode;
            if (!Enum.TryParse(typeof(CurrencyCodeEnum), currencyCode, true, out currentCurrencyCode))
            {
                currentCurrencyCode = CurrencyCodeEnum.None;
            }
            return _exchangeSourceSolver((CurrencyCodeEnum)currentCurrencyCode);
        }


        public async Task<ExchangeRate> GetRateByCurrencyCode(string currencyCode)
        {
            var exchangeRateSource = GetExchangeSourceBySourceCode(currencyCode);
            _logger.LogInformation($"Exchange Source Selected:{exchangeRateSource.GetType().Name}");
            var exchangeRate = await exchangeRateSource.GetRate();
            _logger.LogInformation($"Exchange Rate: {JsonConvert.SerializeObject(exchangeRate)}");
            return exchangeRate;
        }
    }
}
