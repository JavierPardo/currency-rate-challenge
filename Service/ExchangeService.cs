using Service.Interfaces;
using Database.Interfaces;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ExchangeService: IExchangeService
    {
        private Func<CurrencyCodeEnum, IExchangeRateSource> _exchangeSourceSolver;
        public ExchangeService(Func<CurrencyCodeEnum, IExchangeRateSource> exchangeSourceSolver)
        {
            _exchangeSourceSolver = exchangeSourceSolver;
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

        public async Task<ExchangeRate> GetRateByCurrencyCode(string currencyCode)
        {
            object currentCurrencyCode;
            if (!Enum.TryParse(typeof(CurrencyCodeEnum), currencyCode, true, out currentCurrencyCode))
            {
                currentCurrencyCode = CurrencyCodeEnum.None;
            }
            var exchangeRateSource = _exchangeSourceSolver((CurrencyCodeEnum)currentCurrencyCode);
            return await exchangeRateSource.GetRate();
        }
    }
}
