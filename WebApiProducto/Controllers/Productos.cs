using System;
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
        private readonly ILogger<Productos> _logger;
        public Productos(IProductos iproductos_, ILogger<Productos> logger)
        {

            this.iproductos = iproductos_;
            this._logger = logger;
        }
        [HttpGet("AllProductos")]
        public async Task<IResult> GetProductos()
        {
            List<Producto> lsProductos;

            _logger.LogInformation("consultando productos...");
            Task<List<Producto>> lsProducto = iproductos.GetProductos();
            _logger.LogInformation("ejecutando metodos sincronos...");

            lsProductos = await lsProducto;

            _logger.LogInformation("terminó la consulta de productos");
            return Results.Ok(lsProductos);

        }
    }
}