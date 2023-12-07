using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace WebApiProducto.Data
{
     public interface IServiceBus
    {
        Task SendMessageAsync(string title, double precio);
    }

    public class ServiceBus: IServiceBus
    {
        private readonly IConfiguration _configuration;

        public ServiceBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string title, double precio)
        {
            IQueueClient client = new QueueClient(_configuration["ServiceBus:AzureServiceBusConnectionString"], _configuration["ServiceBus:QueueName"]);
            var messageBody = ($"Title: {title}, Precio: {precio}");

            var message = new Message(Encoding.UTF8.GetBytes(messageBody))
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };

            await client.SendAsync(message);
        }
    }
}