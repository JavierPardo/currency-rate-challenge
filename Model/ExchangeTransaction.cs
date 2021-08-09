using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Model
{

    public class ExchangeTransaction
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public decimal AmountInput { get; set; }
        public decimal AmountOutput { get; set; }
        public string CurrencyCodeInput { get; set; }
        public string CurrencyCodeOutput { get; set; }
        public int Status { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; }
    }
}
