using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Identity.API.Data;
using Identity.API.Configurations;
using System.Text;
using Identity.API.Services;
using Microsoft.EntityFrameworkCore;
using Identity.API.Repositories;
using System;
using FluentValidation;
using Identity.API.Validators;
using Identity.API.Payloads.v1.Requests;
using Microsoft.OpenApi.Models;

namespace Identity.API
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
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "IdentityServerDB"));

            services.AddCustomAuthorization(Configuration);

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IValidator<AccountLoginRequest>, AccountLoginRequestValidator>();
            services.AddTransient<IValidator<AccountRegisterRequest>, AccountRegisterRequestValidator>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
