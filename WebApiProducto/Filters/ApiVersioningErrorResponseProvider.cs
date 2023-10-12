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
            ResponseDetailsError errorResponse = new();
            errorResponse.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.1";
            switch (context.StatusCode)
            {
                case 501:
                    errorResponse.Detail = "Esta funcionalidad no está soportada para esta versión de api";
                    errorResponse.Title = ErrorDescription.NoControlado;
                    errorResponse.Status = context.StatusCode;
                    break;
                default:
                    errorResponse.Detail = context.MessageDetail;
                    errorResponse.Title = ErrorDescription.NoControlado;
                    errorResponse.Status = context.StatusCode;
                    break;
            }
            var response = new ObjectResult(errorResponse);
            response.StatusCode = context.StatusCode;

            return response;

        }
    }
}