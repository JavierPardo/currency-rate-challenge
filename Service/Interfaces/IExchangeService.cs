using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IExchangeService
    {
        Task<ExchangeRate> GetRateByCurrencyCode(string currencyCode);
        IAsyncEnumerable<ExchangeRate> GetAllRates();
    }
}
