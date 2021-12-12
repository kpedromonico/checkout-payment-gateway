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
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCustomAuthorization(Configuration);

            services.AddSwagger();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddHttpClient<IIdentityService, IdentityService>(x =>
            {
                if(Environment.IsDevelopment())
                {
                    x.BaseAddress = new Uri("http://localhost:5004/api/");
                }
                else
                {
                    x.BaseAddress = new Uri("http://acme.com/api/");
                }
            });

            services.AddHttpClient<IBankService, BankService>(x => 
            {
                if (Environment.IsDevelopment())
                {
                    x.BaseAddress = new Uri("http://localhost:6000/api/");
                }
                else
                {
                    x.BaseAddress = new Uri("http://acme.com/api/");
                }                
            });

            services.AddHttpClient<IPaymentService, PaymentService>(x =>
            {
                if (Environment.IsDevelopment())
                {
                    x.BaseAddress = new Uri("http://localhost:5000/api/");
                }
                else
                {
                    x.BaseAddress = new Uri("http://acme.com/api/");
                }
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
