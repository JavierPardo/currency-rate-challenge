using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces
{
    public interface IExchangeTransactionService
    {
        ExchangeTransaction Purchase(ExchangeTransaction exchangeTransaction);
    }
}
