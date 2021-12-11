using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Payment.API.Configurations
{
    public static class CircuitBreakerConfiguration
    {
        public static IAsyncPolicy<HttpResponseMessage> BankServicePolicy => Policy
            .HandleResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
            (ex, t) =>
            {
                Console.WriteLine("--> Circuit is broken");
            },
            () => {
                Console.WriteLine("--> Circuit is open");
            });
    }
}
