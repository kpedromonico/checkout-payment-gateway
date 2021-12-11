using FluentValidation;
using Identity.API.Payloads.v1.Requests;

namespace Identity.API.Validators
{
    public class AccountLoginRequestValidator : AbstractValidator<AccountLoginRequest>
    {
        public AccountLoginRequestValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Not a valid email format");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}