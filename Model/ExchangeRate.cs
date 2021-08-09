using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ExchangeRate
    {
        public string CurrencyCode { get; set; }
        public decimal Buy { get; set; }
        public decimal Sell { get; set; }
    }
}
