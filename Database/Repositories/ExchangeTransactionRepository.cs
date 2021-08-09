using Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Repositories
{
    public class ExchangeTransactionRepository: IExchangeTransactionRepository
    {
        private readonly ILogger<ExchangeTransactionRepository> _logger;
        private readonly DatabaseContext _context;
        private readonly DbSet<ExchangeTransaction> _dbSet;

        public ExchangeTransactionRepository(DatabaseContext context, ILogger<ExchangeTransactionRepository> logger)
        {
            _logger = logger;
            _context = context;
            _dbSet = context.Set<ExchangeTransaction>();
        }

        public IEnumerable<ExchangeTransaction> GetAllByMonthCurrencyAndUserId(ExchangeTransaction exchangeTransaction)
        {
            _logger.LogInformation($"Querying information for :{JsonConvert.SerializeObject(exchangeTransaction)}");
            return _dbSet.Where(et => et.UserId == exchangeTransaction.UserId
            && et.DateTime.Month == exchangeTransaction.DateTime.Month
            && et.CurrencyCodeOutput == exchangeTransaction.CurrencyCodeOutput
            && et.Status == (int)ExchangeTransactionStatusEnum.Success).ToList();
        }

        public void Save(ExchangeTransaction exchangeTransaction)
        {
            _logger.LogInformation($"Saving information :{JsonConvert.SerializeObject(exchangeTransaction)}");
            if (exchangeTransaction.Id == Guid.Empty)
            {
                _dbSet.Add(exchangeTransaction);
            }
            else
            {
                _dbSet.Update(exchangeTransaction);
            }
            _context.SaveChanges();
        }
    }
}
