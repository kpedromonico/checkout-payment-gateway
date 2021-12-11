using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void AddCustomMappings(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
