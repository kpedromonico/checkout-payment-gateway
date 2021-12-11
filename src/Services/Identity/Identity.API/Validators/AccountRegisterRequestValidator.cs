using FluentValidation;
using Identity.API.Payloads.v1.Requests;

namespace Identity.API.Validators
{
    public class AccountRegisterRequestValidator : AbstractValidator<AccountRegisterRequest>
    {
        public AccountRegisterRequestValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Not a valid email format");

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("First Name is required");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last Name is required");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must have at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must have at Least one upper case letter")
                .Matches("[a-z]").WithMessage("Password must have at Least one lower case letter")
                .Matches("[0-9]").WithMessage("Password must have at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must have at least one special character");

            RuleFor(p => p.ConfirmPassword)
                .Equal(p => p.Password).WithMessage("Passwords must be equal");
        }
    }
}