using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiProducto.Filters;

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
                Log.Information("prueba:" + Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
                config.OperationFilter<SwaggerDefaultValuesFilter>();
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}