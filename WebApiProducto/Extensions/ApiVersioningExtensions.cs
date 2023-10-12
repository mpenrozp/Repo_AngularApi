using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiProducto.Examples;
using WebApiProducto.Filters;
using WebApiProducto.Models;

namespace WebApiProducto.Extensions
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                // Allows API return versions in the response header (api-supported-versions).
                options.ReportApiVersions = true;
                // Allows to choose whether they would like to place the parameter in the URL or in the request header
                //options.ApiVersionReader = ApiVersionReader.Combine(
                //new UrlSegmentApiVersionReader(),
                //    new HeaderApiVersionReader("x-api-version"));
                //new MediaTypeApiVersionReader("x-api-version"));
                options.ErrorResponses = new ApiVersioningErrorResponseProvider();
            });

            // Allows to discover versions
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(config =>
            {
                config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
                config.OperationFilter<SwaggerDefaultValuesFilter>();
                config.ExampleFilters();
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });
            //services.AddSwaggerExamples();
            services.AddSwaggerExamplesFromAssemblyOf<ProductoResponseExample>();
            services.AddSwaggerExamplesFromAssemblyOf<AutorizacionRequestExample>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddFluentValidationRulesToSwagger();
            //services.AddTransient<IValidator<AutorizacionRequest>, UserValidatorPassword>();
            return services;
        }

    }

}