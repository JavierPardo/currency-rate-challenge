using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is WrongCurrencyException exception)
            {
                context.Result = new ObjectResult($"Currency sent was not found! Please try Again or contact Administrator")
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                };
                context.ExceptionHandled = true;
            }
            if (context.Exception is OverMonthlyLimitException overMontlyLimitException)
            {
                var currencyCode = overMontlyLimitException.CurrencyCode;
                var amount= overMontlyLimitException.Amount;
                context.Result = new ObjectResult($"Can't buy \"{currencyCode}\" the monthly limit is reached with \"{amount}\"")
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                };
                context.ExceptionHandled = true;

            }
        }
    }
}
