using Database.Interfaces;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Database
{
    public class ExchangeRateSourceNotImpl : IExchangeRateSource
    {
        private readonly ILogger<ExchangeRateSourceNotImpl> _logger;

        public ExchangeRateSourceNotImpl(ILogger<ExchangeRateSourceNotImpl> logger)        {
            _logger = logger;
        }
        public decimal GetLimit()
        {
            _logger.LogError("Currency Source Not implemented");
            throw new WrongCurrencyException();
        }

        public Task<ExchangeRate> GetRate(CancellationToken cancelToken = default(CancellationToken))
        {
            _logger.LogError("Currency Source Not implemented");
            throw new WrongCurrencyException();
        }
    }
}
