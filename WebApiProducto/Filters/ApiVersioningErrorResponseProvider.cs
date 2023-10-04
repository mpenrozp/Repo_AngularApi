using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using WebApiProducto.Models;

namespace WebApiProducto.Filters
{
    public class ApiVersioningErrorResponseProvider : DefaultErrorResponseProvider
    {
        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            //You can initialize your own class here. Below is just a sample.
            ResponseDetailsError errorResponse = new()
            {
                Detail = "Esta funcionalidad no está soportada para esta versión de api",
                Title = ErrorDescription.NoControlado,
                Status = context.StatusCode
            };

            var response = new ObjectResult(errorResponse);
            response.StatusCode = context.StatusCode;

            return response;
        }
    }
}