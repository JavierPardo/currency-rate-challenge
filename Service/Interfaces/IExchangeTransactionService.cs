using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IExchangeTransactionService
    {
        Task Purchase(ExchangeTransaction exchangeTransaction);
    }
}
