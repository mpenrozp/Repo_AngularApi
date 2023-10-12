using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApiProducto.Examples;
using WebApiProducto.Models;
using WebApiProducto.Services;

namespace WebApiProducto.Controllers.V2
{
    /// <response code="401">Unauthorized. Usuario no autorizado para acceder a la api.</response>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class Productos : ControllerBase
    {

        private readonly IProductos iproductos;
        private readonly ILogger<Productos> _logger;
        public Productos(IProductos iproductos_, ILogger<Productos> logger)
        {

            this.iproductos = iproductos_;
            this._logger = logger;
        }
        // [ApiExplorerSettings(IgnoreApi = true)]   
        /// <summary>Esta acción devuelve todos los productos</summary>
        /// <remarks>
        /// Devuelve la lista de productos desde una api externa. https://api.escuelajs.co/api/v1/products
        /// </remarks>         
        /// <response code="200">OK. Devuelve la lista de objetos solicitada.</response>        
        /// <response code="500">InternalServerError. Error interno del servidor.</response>
        /// <response code="504">GatewayTimeout. Tiempo de espera agotado para el servicio de consulta de productos.</response>
        [HttpGet("AllProductos")]
        [ProducesResponseType(typeof(List<Producto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status504GatewayTimeout)]
        public async Task<IResult> GetProductosAsync()
        {
            List<Producto> lsProductos;
            string mensajetimer = string.Empty;
            _logger.LogInformation("consultando productos...");
            Task<List<Producto>> lProductos = iproductos.GetProductosAsync();
            Task<string> prueba = iproductos.TimerAsync();

            lsProductos = await lProductos;
            mensajetimer = await prueba;
            return Results.Ok(lsProductos);

        }


    }
}