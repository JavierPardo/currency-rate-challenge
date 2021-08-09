using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class OverMonthlyLimitException : Exception
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }

        public OverMonthlyLimitException(decimal amount, string currencyCode):base()
        {
            Amount = amount;
            CurrencyCode = currencyCode;
        }
    }
}
