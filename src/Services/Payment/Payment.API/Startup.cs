using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Payment.API.Application.Services;
using Payment.API.Configurations;
using Payment.API.Infrastructure.Services;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using Payment.Infrastructure;
using Payment.Infrastructure.Repositories;
using Polly;

namespace Payment.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment.API", Version = "v1" });
            });

            services.AddCustomAuthorization(_configuration);

            services.AddDbContext<PaymentDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "PaymentServerDB"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddCustomMappings();
            services.AddCustomValidators();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddHttpClient<IBankService, BankService>();
            services.AddScoped<IPaymentService, PaymentService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment.API v1"));
            }

            // app.UseHttpsRedirection(); -> Disabling to avoid cert error when inside docker

            app.UseRouting();

            ConfigureAuth(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
