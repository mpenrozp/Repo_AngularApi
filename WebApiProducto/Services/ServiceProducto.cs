using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProducto.Models;
using System.Net.Http;



namespace WebApiProducto.Services
{

    class ServiceProducto : IProductos
    {
        private readonly HttpClient client;
        private readonly ILogger<ServiceProducto> logger;
    
        public ServiceProducto(HttpClient client_, ILogger<ServiceProducto> _logger)
        {
            this.client = client_;
            this.logger = _logger;
        }
        public async Task<List<Producto>> GetProductos()
        {
            List<Producto> lsProductos = new();
            
            try
            {
                HttpResponseMessage response = await this.client.GetAsync("https://api.escuelajs.co/api/v1/products");
                lsProductos = await response.Content.ReadAsAsync<List<Producto>>();
                //Thread.Sleep(20000);
            }
            catch (Exception ex)
            {
                logger.LogInformation("{Message}", ex.Message);
            }
            return lsProductos;


        }
    }
}