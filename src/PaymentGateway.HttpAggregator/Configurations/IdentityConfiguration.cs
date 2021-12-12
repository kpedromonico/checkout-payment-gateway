using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Configurations
{
    public static class IdentityConfiguration
    {
        public static void ConfigureAuthentication(this IApplicationBuilder builder)
        {
            builder.UseAuthentication();
            builder.UseAuthorization();
        }
    }
}
