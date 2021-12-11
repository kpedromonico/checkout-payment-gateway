using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Payment.API.Application.Payloads.Requests;

namespace Payment.API.Application.Validators
{
    public class PaymentAttemptRequestValidator : AbstractValidator<PaymentAttemptRequest>
    {
        public PaymentAttemptRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(decimal.Zero)
                .WithMessage("The transaction amount needs to be greater than 0.00");

            RuleFor(x => x.CardNumber)
                .CreditCard()
                .NotEmpty()
                .WithMessage("Card Number is invalid");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("The currency can't be empty");

            RuleFor(x => x.Cvv)
                .GreaterThan(0)
                .Must(x => (Math.Floor(Math.Log10(x) + 1) == 3)) 
                .WithMessage("Security number needs to contain 3 digits");

            RuleFor(x => new { x.ExpiryMonth, x.ExpiryYear })
                .Must(y => ValidateExpirationDate(y.ExpiryMonth, y.ExpiryYear))
                .WithMessage("The card is expired or the expiration date is invalid");            
        }

        private bool ValidateExpirationDate(int month, int year)
        {
            if(year <= 0 || month <= 0 || month > 12 || year < DateTime.Today.Year)
                return false;

            if (year == DateTime.Today.Year && month < DateTime.Today.Month)
                return false;

            return true;
        }
    }
}
