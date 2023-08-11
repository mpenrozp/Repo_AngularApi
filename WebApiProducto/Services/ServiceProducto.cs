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
        private readonly IConfiguration configuration;
    
        public ServiceProducto(HttpClient client_, ILogger<ServiceProducto> _logger, IConfiguration _configuration)
        {
            this.client = client_;
            this.logger = _logger;
            this.configuration = _configuration;
        }
        public async Task<List<Producto>> GetProductos()
        {
            List<Producto> lsProductos = new();
            
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(configuration["urlGetImages"]);
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