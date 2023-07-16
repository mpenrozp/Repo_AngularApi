using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiProducto.Models;
using WebApiProducto.Services;

namespace WebApiProducto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class Productos : ControllerBase
    {

        private readonly IProductos iproductos;
        private readonly ILogger<Productos> logger;
        public Productos(IProductos iproductos_,ILogger<Productos> _logger)
        {

            this.iproductos = iproductos_;
            this.logger =_logger;
        }
        [HttpGet("AllProductos")]
        public async Task<IActionResult>  GetProductos()
        {
            List<Producto> lsProductos;
            //Producto oProd = new Producto(){ Id= 1, Price = 2000, Title = "Titulo", Images = new string[100] };
            logger.LogInformation("consultando productos...");
            Task<List<Producto>> lsProducto = iproductos.GetProductos();
            logger.LogInformation("ejecutando metodos sincronos...");

             lsProductos = await lsProducto;

             logger.LogInformation("terminó la consulta de productos");
             return Ok(lsProductos);
        } 
    }
}