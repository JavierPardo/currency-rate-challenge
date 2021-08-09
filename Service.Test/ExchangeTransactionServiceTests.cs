using Database.Interfaces;
using Infrastructure;
using Model.Enum;
using Moq;
using NUnit.Framework;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Test
{
    public class ExchangeTransactionServiceTests
    {
        ExchangeTransactionService _exchangeTransactionService;
        Mock<IExchangeRateSource> _mockExchangeRateSource = new Mock<IExchangeRateSource>();
        Mock<IExchangeTransactionRepository> _mockExchangeTransactionRepository = new Mock<IExchangeTransactionRepository>();
        [SetUp]
        public void Setup()
        {

            Func<CurrencyCodeEnum, IExchangeRateSource> func = key => _mockExchangeRateSource.Object;
            _exchangeTransactionService = new ExchangeTransactionService(_mockExchangeTransactionRepository.Object, func);
        }


        [Test]
        public async Task GivenAUnderLimitAmount_WhenTryToBuyCurrency_ThenReturnsACorrectOutPut()
        {
            var expectedStatus = (int)ExchangeTransactionStatusEnum.Success;
            var expectedAmount = 150;
            var expectedCurrencyInput = "ARS";
            var expectedCurrencyOutput = "USD";

            _mockExchangeRateSource.Setup(e => e.GetRate(default(System.Threading.CancellationToken)))
                .ReturnsAsync(new Model.ExchangeRate
                {
                    Sell = 10
                });
            _mockExchangeRateSource.Setup(e => e.GetLimit()).Returns(1000);

            var actual = new Model.ExchangeTransaction
            {
                AmountInput = 1500,
                CurrencyCodeOutput = "USD",
                UserId = 3
            };
            await _exchangeTransactionService.Purchase(actual);

            Assert.AreEqual(expectedAmount, actual.AmountOutput);
            Assert.AreEqual(expectedCurrencyInput, actual.CurrencyCodeInput);
            Assert.AreEqual(expectedCurrencyOutput, actual.CurrencyCodeOutput);
            Assert.AreEqual(expectedStatus, actual.Status);
            Assert.Pass();
        }

        [Test]
        public void GivenAOverLimitAmount_WhenTryToBuyCurrency_ThenThrowException()
        {
            var expectedStatus = (int)ExchangeTransactionStatusEnum.Rejected;
            var expectedAmount = 150;
            var expectedCurrencyInput = "ARS";
            var expectedCurrencyOutput = "USD";

            _mockExchangeRateSource.Setup(e => e.GetRate(default(System.Threading.CancellationToken)))
                .ReturnsAsync(new Model.ExchangeRate
                {
                    Sell = 10
                });
            _mockExchangeRateSource.Setup(e => e.GetLimit()).Returns(1);

            var exchangeTransaction = new Model.ExchangeTransaction
            {
                AmountInput = 1500,
                CurrencyCodeOutput = "USD",
                UserId = 3
            };
            var actual = Assert.ThrowsAsync<OverMonthlyLimitException>(async () => await _exchangeTransactionService.Purchase(exchangeTransaction));

            Assert.AreEqual(typeof(OverMonthlyLimitException), actual.GetType());

            Assert.AreEqual(expectedAmount, exchangeTransaction.AmountOutput);
            Assert.AreEqual(expectedCurrencyInput, exchangeTransaction.CurrencyCodeInput);
            Assert.AreEqual(expectedCurrencyOutput, exchangeTransaction.CurrencyCodeOutput);
            Assert.AreEqual(expectedStatus, exchangeTransaction.Status);
            Assert.Pass();
        }

        [Test]
        public void GivenAnAmount_WhenMonthlyAmountIsExceed_ThenThrowException()
        {
            var expectedStatus = (int)ExchangeTransactionStatusEnum.Rejected;
            var expectedAmount = 150;
            var expectedCurrencyInput = "ARS";
            var expectedCurrencyOutput = "USD";

            _mockExchangeRateSource.Setup(e => e.GetRate(default(System.Threading.CancellationToken)))
                .ReturnsAsync(new Model.ExchangeRate
                {
                    Sell = 10
                });
            _mockExchangeRateSource.Setup(e => e.GetLimit()).Returns(151);
            
            var exchangeTransaction = new Model.ExchangeTransaction
            {
                AmountInput = 1500,
                CurrencyCodeOutput = "USD",
                UserId = 3
            };
            _mockExchangeTransactionRepository.Setup(r => r.GetAllByMonthCurrencyAndUserId(exchangeTransaction))
                .Returns(new List<Model.ExchangeTransaction>
                {
                    new Model.ExchangeTransaction
                    {
                        AmountOutput=2
                    }
                });

            var actual = Assert.ThrowsAsync<OverMonthlyLimitException>(async () => await _exchangeTransactionService.Purchase(exchangeTransaction));

            Assert.AreEqual(typeof(OverMonthlyLimitException), actual.GetType());

            Assert.AreEqual(expectedAmount, exchangeTransaction.AmountOutput);
            Assert.AreEqual(expectedCurrencyInput, exchangeTransaction.CurrencyCodeInput);
            Assert.AreEqual(expectedCurrencyOutput, exchangeTransaction.CurrencyCodeOutput);
            Assert.AreEqual(expectedStatus, exchangeTransaction.Status);
            Assert.Pass();
        }
    }
}
