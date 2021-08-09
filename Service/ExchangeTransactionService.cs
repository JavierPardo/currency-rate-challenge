using Database.Interfaces;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Model;
using Model.Enum;
using Newtonsoft.Json;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ExchangeTransactionService : IExchangeTransactionService
    {
        private readonly ILogger<ExchangeTransactionService> _logger;
        private readonly Func<CurrencyCodeEnum, IExchangeRateSource> _exchangeSourceSolver;
        private readonly IExchangeTransactionRepository _exchangeTransactionRepository;

        public ExchangeTransactionService(IExchangeTransactionRepository exchangeTransactionRepository,
            Func<CurrencyCodeEnum, IExchangeRateSource> exchangeSourceSolver,
            ILogger<ExchangeTransactionService> logger)
        {
            _logger = logger;
            _exchangeSourceSolver = exchangeSourceSolver;
            _exchangeTransactionRepository = exchangeTransactionRepository;
        }

        private IExchangeRateSource GetExchangeSourceBySourceCode(string currencyCode)
        {
            _logger.LogInformation($"Getting Source for:{currencyCode}");
            object currentCurrencyCode;
            if (!Enum.TryParse(typeof(CurrencyCodeEnum), currencyCode, true, out currentCurrencyCode))
            {
                currentCurrencyCode = CurrencyCodeEnum.None;
            }
            return _exchangeSourceSolver((CurrencyCodeEnum)currentCurrencyCode);
        }
        public async Task Purchase(ExchangeTransaction exchangeTransaction)
        {
            _logger.LogInformation($"Purchasing :{JsonConvert.SerializeObject(exchangeTransaction)}");
            var exchangeSource = GetExchangeSourceBySourceCode(exchangeTransaction.CurrencyCodeOutput);

            _logger.LogInformation($"Exchange Source Selected :{JsonConvert.SerializeObject(exchangeSource.GetType().Name)}");
            var exchangeRate = await exchangeSource.GetRate();

            exchangeTransaction.AmountOutput = exchangeTransaction.AmountInput / exchangeRate.Sell;
            exchangeTransaction.CurrencyCodeInput = "ARS";
            exchangeTransaction.DateTime = DateTime.Now;
            _logger.LogInformation($"Transaction after conversion:{JsonConvert.SerializeObject(exchangeTransaction)}");


            var validTransactionMonthly = _exchangeTransactionRepository.GetAllByMonthCurrencyAndUserId(exchangeTransaction);
            var currentMontlyAmount = validTransactionMonthly.Aggregate((decimal)0, (result, trans) => (result + trans.AmountOutput));
            var newMontlyAmount = exchangeTransaction.AmountOutput + currentMontlyAmount;
            var monthlyLimit = exchangeSource.GetLimit();

            if (newMontlyAmount > monthlyLimit)
            {
                _logger.LogError($"Amount Exceed monthly Limit, current total: {currentMontlyAmount}, current amount:{exchangeTransaction.AmountOutput}, curren limit:{monthlyLimit}");

                exchangeTransaction.Status = (int)ExchangeTransactionStatusEnum.Rejected;
                exchangeTransaction.Comment = $"Transaction rejected, User has reached the monthly limit for \"{exchangeTransaction.AmountInput}\" to  \"{exchangeTransaction.CurrencyCodeOutput}\"";
                _exchangeTransactionRepository.Save(exchangeTransaction);
                _logger.LogError($"Transaction Saved: {JsonConvert.SerializeObject(exchangeTransaction)}");

                throw new OverMonthlyLimitException(exchangeTransaction.AmountOutput, exchangeTransaction.CurrencyCodeOutput);
            }
            else
            {
                _logger.LogInformation($"Purchase successfuly Processed!");

                exchangeTransaction.Status = (int)ExchangeTransactionStatusEnum.Success;
                exchangeTransaction.Comment = $"Transaction Successfully.";
                _logger.LogInformation($"Transaction Saved: {JsonConvert.SerializeObject(exchangeTransaction)}");
                _exchangeTransactionRepository.Save(exchangeTransaction);
            }
        }
    }
}
