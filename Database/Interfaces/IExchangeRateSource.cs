using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IExchangeRateSource
    {
        Task<ExchangeRate> GetRate(CancellationToken cancelToken = default(CancellationToken));
        decimal GetLimit();
    }
}
