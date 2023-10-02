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
        private readonly IHttpClientFactory _client;
        private readonly HttpClient _client2;
        private readonly ILogger<ServiceProducto> _logger;
        private readonly IConfiguration _configuration;

        public ServiceProducto(IHttpClientFactory  client, HttpClient client2, ILogger<ServiceProducto> logger, IConfiguration configuration)
        {
            this._client = client;
            this._logger = logger;
            this._client2 = client2;
            this._configuration = configuration;
            this._client2.BaseAddress = new Uri("https://api.escuelajs.co/");
            this._client2.Timeout = TimeSpan.FromSeconds(1);
        }
        public async Task<List<Producto>> GetProductos()
        {
            List<Producto> lsProductos = new();
            var httpClient = _client.CreateClient("GetImagenes");
            //this.client.Timeout = TimeSpan.FromSeconds(1);
            HttpResponseMessage response = await httpClient.GetAsync("api/v1/products");//configuration["urlGetImages"]"
            response.EnsureSuccessStatusCode();
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
            HttpResponseMessage response = await this._client2.GetAsync(_configuration["urlGetImages"]);//configuration["urlGetImages"]"

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