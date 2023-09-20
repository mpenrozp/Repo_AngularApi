using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProducto.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace WebApiProducto.Services
{

    class ServiceProducto : IProductos
    {
        private readonly IHttpClientFactory client;
        private readonly HttpClient client2;
        private readonly ILogger<ServiceProducto> logger;
        private readonly IConfiguration configuration;

        public ServiceProducto(IHttpClientFactory  client_, HttpClient client_2, ILogger<ServiceProducto> _logger, IConfiguration _configuration)
        {
            this.client = client_;
            this.logger = _logger;
            this.configuration = _configuration;
            this.client2 = client_2;
            //this.client.BaseAddress = new Uri("https://api.escuelajs.co/");
            //this.client.Timeout = TimeSpan.FromSeconds(1);
        }
        public async Task<List<Producto>> GetProductos()
        {
            List<Producto> lsProductos = new();
            var httpClient = client.CreateClient("GetImagenes");
            //this.client.Timeout = TimeSpan.FromSeconds(1);
            HttpResponseMessage response = await httpClient.GetAsync("api/v1/products");//configuration["urlGetImages"]"

            // lsProductos = await response.Content.ReadAsAsync<List<Producto>>();
            if (response.IsSuccessStatusCode)
            {
                string resp = await response.Content.ReadAsStringAsync();
                lsProductos = JsonConvert.DeserializeObject<List<Producto>>(resp)!;
            }

            // Thread.Sleep(20000);

            return lsProductos!;


        }
        public async Task<List<Producto>> GetProductos2()
        {
            List<Producto> lsProductos = new();
           //var httpClient = client.CreateClient("GetImagenes");
            //this.client.Timeout = TimeSpan.FromSeconds(1);
            HttpResponseMessage response = await this.client2.GetAsync(configuration["urlGetImages"]);//configuration["urlGetImages"]"

            // lsProductos = await response.Content.ReadAsAsync<List<Producto>>();
            if (response.IsSuccessStatusCode)
            {
                string resp = await response.Content.ReadAsStringAsync();
                lsProductos = JsonConvert.DeserializeObject<List<Producto>>(resp)!;
            }

            // Thread.Sleep(20000);

            return lsProductos!;


        }
    }
}