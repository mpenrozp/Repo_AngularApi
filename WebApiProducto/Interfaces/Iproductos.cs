using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProducto.Models;

namespace WebApiProducto.Services
{
    public interface IProductos
    {
        Task<List<Producto>> GetProductos();
        Task<List<Producto>> GetProductos2();
    }
}