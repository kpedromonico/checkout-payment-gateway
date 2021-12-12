using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PaymentGateway.HttpAggregator.Configurations;
using PaymentGateway.HttpAggregator.Data;
using PaymentGateway.HttpAggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCustomAuthorization(Configuration);

            services.AddSwagger();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>(); // drop

            services.AddHttpClient<IIdentityService, IdentityService>(x =>
            {
                //x.BaseAddress = new Uri("http://localhost:5004/api/");
                x.BaseAddress = new Uri("http://acme.com/api/");
            });

            services.AddHttpClient<IBankService, BankService>(x => 
            {
                x.BaseAddress = new Uri("http://acme.com/api/");
                //x.BaseAddress = new Uri("http://localhost:6000/api/");
            });

            services.AddHttpClient<IPaymentService, PaymentService>(x =>
            {
                x.BaseAddress = new Uri("http://acme.com/api/");
                //x.BaseAddress = new Uri("http://localhost:6000/api/");
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCustomSwagger();

            // app.UseHttpsRedirection(); there is no valid certificate on the k8s deploy, so commenting this out
            app.UseStaticFiles();

            app.UseRouting();

            app.ConfigureAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
