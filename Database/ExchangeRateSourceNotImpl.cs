using Database.Interfaces;
using Infrastructure;
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
        public Task<ExchangeRate> GetRate(CancellationToken cancelToken = default(CancellationToken))
        {
            throw new WrongCurrencyException();
        }
    }
}
