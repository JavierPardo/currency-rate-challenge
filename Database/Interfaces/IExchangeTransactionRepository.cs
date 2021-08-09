using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Interfaces
{
    public interface IExchangeTransactionRepository
    {
        IEnumerable<ExchangeTransaction> GetAllByMonthCurrencyAndUserId(ExchangeTransaction exchangeTransaction);
        void Save(ExchangeTransaction exchangeTransaction);
    }
}
