using System;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiProducto.Models;
using WebApiProducto.Services;

namespace WebApiProducto.Controllers.V2
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Productos : ControllerBase
    {

        private readonly IProductos iproductos;
        private readonly ILogger<Productos> _logger;
        public Productos(IProductos iproductos_, ILogger<Productos> logger)
        {

            this.iproductos = iproductos_;
            this._logger = logger;
        }
        [HttpGet("AllProductos")]
        [MapToApiVersion("2.0")]
        public IResult GetProductos()
        {
             throw new HttpRequestException("Método no implementado en esta versión", new Exception(), HttpStatusCode.NotImplemented);

        }

    }
}