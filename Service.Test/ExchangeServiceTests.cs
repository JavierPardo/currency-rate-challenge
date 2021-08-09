using Database.Interfaces;
using Infrastructure;
using Model;
using Model.Enum;
using Moq;
using NUnit.Framework;
using Service.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Test
{
    public class ExchangeServiceTests
    {
        private Mock<IExchangeRateSource> _exchangeRateSource;
        private Mock<IExchangeRateSource> _notImplementedRateSource;
        private ExchangeService _exchangeService;

        [SetUp]
        public void Setup()
        {
            _exchangeRateSource = new Mock<IExchangeRateSource>();
            _notImplementedRateSource = new Mock<IExchangeRateSource>();
            Func<CurrencyCodeEnum, IExchangeRateSource> func = key =>
            {
                switch (key)
                {
                    case CurrencyCodeEnum.BRL:
                    case CurrencyCodeEnum.USD:
                        return _exchangeRateSource.Object;
                    default:
                        return _notImplementedRateSource.Object;
                }
            };
            _exchangeService = new ExchangeService(func);
        }

        [Test]
        public async Task GivenAValidCurrencyRequestRate_WhenExistsImplementation_ThenItShouldReturnAValidResponse()
        {
            var expected = new ExchangeRate
            {
                Buy = 15,
                Sell = 20,
                CurrencyCode = "USD"
            };
            _exchangeRateSource
                .Setup<Task<ExchangeRate>>(p => p.GetRate(default(CancellationToken)))
                .ReturnsAsync(expected);

            var actual = await _exchangeService.GetRateByCurrencyCode("USD");
            Assert.AreEqual(expected.Buy, actual.Buy);
            Assert.AreEqual(expected.Sell, actual.Sell);
            Assert.AreEqual(expected.CurrencyCode, actual.CurrencyCode);
            Assert.Pass();
        }

        [Test]
        public void GivenAnInvalidCurrencyRequestRate_WhenNotExistsImplementation_ThenItShouldReturnAnExceptionResponse()
        {
            _notImplementedRateSource
                .Setup<Task<ExchangeRate>>(p => p.GetRate(default(CancellationToken)))
                .Throws(new WrongCurrencyException());

            var actual = Assert.ThrowsAsync<WrongCurrencyException>(async () => await _exchangeService.GetRateByCurrencyCode("NOT"));
            Assert.AreEqual(typeof(WrongCurrencyException), actual.GetType());
            Assert.Pass();
        }

        [Test]
        public async Task GivenAllCurrenciesRates_WhenExistsImplementation_ThenItOnlyImplementedRates()
        {
            var validRate = new ExchangeRate{};

            _exchangeRateSource
                .Setup<Task<ExchangeRate>>(p => p.GetRate(default(CancellationToken)))
                .ReturnsAsync(validRate);

            _notImplementedRateSource
                .Setup<Task<ExchangeRate>>(p => p.GetRate(default(CancellationToken)))
                .Throws(new WrongCurrencyException());
            var expected = 2;
            var actual = 0;
            await foreach (var rate in _exchangeService.GetAllRates()) {
                actual++;
            }
            Assert.AreEqual(expected, actual);

        }
    }        
}