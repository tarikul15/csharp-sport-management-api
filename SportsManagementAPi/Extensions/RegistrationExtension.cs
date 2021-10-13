using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace SportsManagementAPi.Extensions
{
    public static class RegistrationExtension
    {
        private const string OpenApiInfoTitle = "Sport Management API";
        internal const string ApiVersion = "v1";

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = ApiVersion,
                    Title = OpenApiInfoTitle,
                    Description = $"{OpenApiInfoTitle} provides endpoints to create managers and login using jwt bearer authentication and provides endpoints to manage teams, players, schedule and results "
                });

                c.ExampleFilters();
                c.OperationFilter<AddResponseHeadersFilter>();
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();

        }

    }
}
