using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiProducto.Models;
using WebApiProducto.Services;

namespace WebApiProducto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class Productos : ControllerBase
    {

        private readonly IProductos iproductos;
        private readonly ILogger<Productos> logger;
        public Productos(IProductos iproductos_, ILogger<Productos> _logger)
        {

            this.iproductos = iproductos_;
            this.logger = _logger;
        }
        [HttpGet("AllProductos")]
        public async Task<IResult> GetProductos()
        {
            List<Producto> lsProductos;
            try
            {
                //Producto oProd = new Producto(){ Id= 1, Price = 2000, Title = "Titulo", Images = new string[100] };
                logger.LogInformation("consultando productos...");
                Task<List<Producto>> lsProducto = iproductos.GetProductos();
                logger.LogInformation("ejecutando metodos sincronos...");

                lsProductos = await lsProducto;

                logger.LogInformation("terminó la consulta de productos");
                return Results.Ok(lsProductos);
            }
            catch (HttpRequestException tex)
            {
                logger.LogError(tex, " Error: {Message}, {StackTrace} ", tex.Message, tex.StackTrace);
                return Results.Problem(tex.Message, null, tex.StatusCode.HasValue ? (int)tex.StatusCode! : StatusCodes.Status500InternalServerError, ErrorDescription.NoControlado);
            }
            catch (TaskCanceledException tex2)
            {
                logger.LogError(tex2, " Error: {Message}, {StackTrace} ", tex2.Message, tex2.StackTrace);
                return Results.Problem(tex2.Message, null, StatusCodes.Status504GatewayTimeout, ErrorDescription.NoControlado);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, " Error: {Message}, {StackTrace} ", ex.Message, ex.StackTrace);
                return Results.Problem(ex.Message, null, null, ErrorDescription.NoControlado);
            }


        }
    }
}