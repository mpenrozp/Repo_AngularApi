using System;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebApiProducto.Models;
using WebApiProducto.Services;

namespace WebApiProducto.Controllers.V1
{
    /// <summary>Este controlador gestiona el CRUD de todos los productos</summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("1.0")]
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
        // [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Producto>>> GetProductos()
        {
            List<Producto> lsProductos;

            _logger.LogInformation("consultando productos...");
            Task<List<Producto>> lsProducto = iproductos.GetProductos();
            _logger.LogInformation("ejecutando metodos sincronos...");

            lsProductos = await lsProducto;

            _logger.LogInformation("terminó la consulta de productos");
            return Ok(lsProductos);

        }


    }
}