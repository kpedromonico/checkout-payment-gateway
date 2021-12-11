using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Payment.API.Application.Payloads.Requests;
using Payment.API.Application.Validators;

namespace Payment.API.Configurations
{
    public static class ValidatorsConfiguration 
    {
        public static void AddCustomValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<PaymentAttemptRequest>, PaymentAttemptRequestValidator>();
        }
    }
}
