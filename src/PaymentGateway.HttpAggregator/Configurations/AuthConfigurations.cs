using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PaymentGateway.HttpAggregator.Configurations
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }
    }

    public static class AuthConfigurations
    {
        public static void AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
        {   
            services.AddAuthentication(opt =>
            {
                opt.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = GetTokenValidationParameters(configuration);
                opt.RequireHttpsMetadata = false; // Since there is no certificate
                opt.SaveToken = true;
            });
        }

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = "payments",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };
        }
    }
}
