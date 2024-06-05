using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiProducto.Filters
{
    public class SwaggerDefaultValuesFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p =>
                    p.Name.Equals(parameter.Name, StringComparison.CurrentCultureIgnoreCase));

                parameter.Description ??= description.ModelMetadata?.Description;

                if (parameter.Schema.Default is null && description.DefaultValue is not null)
                {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }

                parameter.Required |= description.IsRequired;
            }
         /*   var actionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (!string.Equals(actionDescriptor.ActionName, "Login", StringComparison.InvariantCultureIgnoreCase))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-Authorization",
                    In = ParameterLocation.Header,
                    Description = "access token",
					Required = true,
					Schema = new OpenApiSchema {
						Type = "string",
						Default = new OpenApiString("Bearer {access token}"),
					}
                });
            }*/
        }
    }
}