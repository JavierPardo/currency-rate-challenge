using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeTransactionController : ControllerBase
    {
        private readonly IExchangeTransactionService _exchangeTransactionService;

        public ExchangeTransactionController(IExchangeTransactionService exchangeTransactionService)
        {
            _exchangeTransactionService = exchangeTransactionService;
        }

        // POST api/<ExchangeTransaction>
        [HttpPost]
        public async Task<Model.ExchangeTransaction> Post([FromBody] Model.ExchangeTransaction exchangeTransaction)
        {
            await _exchangeTransactionService.Purchase(exchangeTransaction);
            
            return exchangeTransaction;
        }
    }
}
