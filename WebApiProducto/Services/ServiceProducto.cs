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
        public ServiceProducto(HttpClient client_)
        {
            this.client = client_;
        }
        public async Task<List<Producto>> GetProductos()
        {
            List<Producto> lsProductos = new();
            var timer = new System.Timers.Timer(200000);
            timer.Start();
            
            try
            {
                HttpResponseMessage response = await this.client.GetAsync("https://api.escuelajs.co/api/v1/products");
                lsProductos = await response.Content.ReadAsAsync<List<Producto>>();

                // RestApi = wcAPI.UploadString(wsUrl, "POST", jsonSerializer);
                //lsProductos = JsonConvert.DeserializeObject<ResponseOKServiceProvider>(responseBody);
            }
            catch (Exception ex)
            {
                
            }
            return lsProductos;


        }
    }
}