using Database.Interfaces;
using Model;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class ExchangeTransactionService : IExchangeTransactionService
    {
        private readonly IExchangeTransactionRepository _exchangeTransactionRepository;

        public ExchangeTransactionService(IExchangeTransactionRepository exchangeTransactionRepository)
        {
            _exchangeTransactionRepository = exchangeTransactionRepository;
        }
        public ExchangeTransaction Purchase(ExchangeTransaction exchangeTransaction)
        {
            var validTransactionMonthly = _exchangeTransactionRepository.GetAllByMonthAndUserId(exchangeTransaction.UserId, DateTime.Now.Month);
            throw new NotImplementedException();
        }
    }
}
