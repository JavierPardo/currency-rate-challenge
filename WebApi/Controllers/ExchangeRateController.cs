using Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeRateController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        // GET api/<ExchangeController>/5
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_exchangeService.GetAllRates());
        }

        // GET api/<ExchangeController>/5
        [HttpGet("{currencyCode}")]
        public IActionResult Get(string currencyCode)
        {
            return Ok(_exchangeService.GetRateByCurrencyCode(currencyCode));
        }
    }
}
