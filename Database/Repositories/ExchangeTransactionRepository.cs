using Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.Enum;
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

        public IEnumerable<ExchangeTransaction> GetAllByMonthAndUserId(long userId, int month)
        {
            return _dbSet.Where(et => et.UserId == userId
            && et.DateTime.Month == month
            && et.Status == (int)ExchangeTransactionStatus.Success);
        }
    }
}
