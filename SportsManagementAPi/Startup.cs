using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SportsManagementAPi.Domain.Repositories;
using SportsManagementAPi.Domain.Security;
using SportsManagementAPi.Domain.Services;
using SportsManagementAPi.Extensions;
using SportsManagementAPi.Repositories;
using SportsManagementAPi.Security;
using SportsManagementAPi.Services;
using Swashbuckle.AspNetCore.SwaggerUI;


namespace SportsManagementAPi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("SportManagementApi");
            });

            services.AddControllers().AddNewtonsoftJson();
            services.AddHealthChecks();
            //services.AddCustomSwagger();

            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<ISportManagementRepository, SportManagementRepository>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenHandler, SportsManagementAPi.Security.TokenHandler>();

            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<ISportManagementService, SportManagementService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.Configure<TokenOptions>(Configuration.GetSection("TokenOptions"));
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            var signingConfigurations = new SigningConfigurations(tokenOptions.Secret);
            services.AddSingleton(signingConfigurations);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        IssuerSigningKey = signingConfigurations.SecurityKey,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAutoMapper(this.GetType().Assembly);
            services.ConfigureSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            //app.UseCustomSwagger();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/isalive");
            });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pbp.Payments.Pas.Cloud.Api");
                c.SupportedSubmitMethods(SubmitMethod.Get);
            });

        }
    }

}
