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
using WebApiProducto.Data;
using System.ComponentModel.DataAnnotations;
using Serilog;
using Microsoft.AspNetCore.Authorization.Infrastructure;



namespace WebApiProducto.Controllers.V2
{
    /// <response code="401">Unauthorized. Usuario no autorizado para acceder a la api.</response>
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class Productos : ControllerBase
    {

        private readonly IProductos iproductos;
        private readonly ILogger<Productos> _logger;
        private readonly IServiceBus iServicesBus;
        private readonly IConfiguration _configuration;

        public Productos(IProductos iproductos_, ILogger<Productos> logger, IServiceBus _iServicesBus, IConfiguration configuration)
        {

            this.iproductos = iproductos_;
            this._logger = logger;
            this.iServicesBus = _iServicesBus;
            this._configuration = configuration;
        }
        [HttpGet("AllProductos")]
        public async Task<IActionResult> GetProductos()
        {
            List<Producto> lsProductos;
            //Producto oProd = new Producto(){ Id= 1, Price = 2000, Title = "Titulo", Images = new string[100] };
            _logger.LogInformation("consultando productos...");
            Task<List<Producto>> lsProducto = iproductos.GetProductosAsync();
            _logger.LogInformation("ejecutando metodos sincronos...");

            lsProductos = await lsProducto;

            _logger.LogInformation("terminó la consulta de productos");
            return Ok(lsProductos);
        }
        // [ApiExplorerSettings(IgnoreApi = true)]   
        /// <summary>Esta acción devuelve todos los productos</summary>
        /// <remarks>
        /// Devuelve la lista de productos desde una api externa. https://api.escuelajs.co/api/v1/products
        /// </remarks>         
        /// <response code="200">OK. Devuelve la lista de objetos solicitada.</response>        
        /// <response code="500">InternalServerError. Error interno del servidor.</response>
        /// <response code="504">GatewayTimeout. Tiempo de espera agotado para el servicio de consulta de productos.</response>
        [HttpGet("getAllProducts")]
        [ProducesResponseType(typeof(List<Producto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status504GatewayTimeout)]
        public async Task<IResult> GetAllProductsAsync()
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
        /// <summary>Esta acción devuelve un producto</summary>
        /// <remarks>
        /// Devuelve el producto solicitado.
        /// </remarks>         
        /// <response code="200">OK. Devuelve la lista de objetos solicitada.</response>        
        /// <response code="500">InternalServerError. Error interno del servidor.</response>
        /// <response code="504">GatewayTimeout. Tiempo de espera agotado para el servicio de consulta de productos.</response>
        /// <response code="404">NotFound. Producto no encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status404NotFound)]
        public async Task<IResult> GetProductAsync(int id)
        {
            List<Producto> lsProductos;
            Producto prod;
            _logger.LogInformation("consultando productos...");
            Task<List<Producto>> lProductos = iproductos.GetProductosAsync();

            lsProductos = await lProductos;
            prod = lsProductos.Where(a => a.Id == id).FirstOrDefault()!;
            if (prod == null)
                throw new HttpRequestException("Producto no encontrado!", new Exception(), HttpStatusCode.NotFound);
            return Results.Ok(prod);
        }
        /// <summary>Esta acción agrega un nuevo producto</summary>
        /// <remarks>
        /// Devuelve el mismo producto agregado
        /// </remarks>         
        /// <response code="200">OK. Devuelve el mimso producto agregado.</response>        
        /// <response code="500">InternalServerError. Error interno del servidor.</response>
        [HttpPost("CreateNewProductMessage")]
        [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status500InternalServerError)]
        public async Task<IResult> CreateNewProductMessageAsync(Producto producto)
        {
             await iServicesBus.SendMessageAsync(producto.Title, producto.Price);
             return Results.Ok();
            
        }
        /// <summary>Esta acción agrega un nuevo producto</summary>
        /// <remarks>
        /// Devuelve el mismo producto agregado
        /// </remarks>         
        /// <response code="200">OK. Devuelve el mimso producto agregado.</response>        
        /// <response code="500">InternalServerError. Error interno del servidor.</response>
        [HttpPost("CreateNewProductMessageCredentials")]
        [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status500InternalServerError)]
        public async Task<IResult> CreateNewProductMessageAsyncCredentials(Producto producto)
        {
            await iServicesBus.SendMessageAsyncDefaultCreden(producto.Title, producto.Price);
            return Results.Ok();

        }
        /// <summary>Esta acción modifica un producto</summary>
        /// <remarks>
        /// Devuelve el mismo producto modificado
        /// </remarks>         
        /// <response code="200">OK. Devuelve el mismo producto modificado.</response>        
        /// <response code="500">InternalServerError. Error interno del servidor.</response>
        [HttpPut("updateProduct")]
        [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status500InternalServerError)]
        public async Task<Producto> UpdateProductAsync(Producto producto)
        {
            await Task.Delay(1000);
            throw new NotImplementedException();
        }
        /// <summary>Esta acción elimina un producto</summary>
        /// <remarks>
        /// Devuelve el producto eliminado
        /// </remarks>         
        /// <response code="200">OK. Devuelve el mismo producto eliminado.</response>        
        /// <response code="500">InternalServerError. Error interno del servidor.</response>
        [HttpDelete("deleteProduct")]
        [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status500InternalServerError)]
        public async Task<Producto> DeleteProductAsync(Producto producto)
        {
            await Task.Delay(1000);
            throw new NotImplementedException();
        }
        [HttpGet("GetSecreto")]
        public async Task<IResult> GetSecreto(string nombre)
        {
            await Task.Delay(1000);
            return Results.Ok(_configuration[nombre]);

        }
        [HttpPut("UpdateMail")]
        [ProducesResponseType(typeof(ResponseDetailsError), StatusCodes.Status500InternalServerError)]
        public async Task<IResult> UpdateMailAsync([FromHeader(Name = "Authorization")] string auth ,
            [Required]UpdateMailRequest updateMailRequest)
        {
            await Task.Delay(1000);
            _logger.LogInformation($" Identificador: { updateMailRequest.Identificador }, Mail: { updateMailRequest.Mail} , Token: {auth}");
            return Results.Ok("Mail modificado correctamente!");
        }

    }
}