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
            // Put in a diff file later
            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "IdentityServerDB"));

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IValidator<AccountLoginRequest>, AccountLoginRequestValidator>();
            services.AddTransient<IValidator<AccountRegisterRequest>, AccountRegisterRequestValidator>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.SaveToken = false;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = tokenValidationParameters;
            });

            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Server", Version = "v1" });
            // });

            services.AddSingleton<WeatherForecastService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer"));
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
