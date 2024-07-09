using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Azure.ServiceBus;
using Azure.Messaging.ServiceBus;



namespace WebApiProducto.Data
{
     public interface IServiceBus
    {
        Task SendMessageAsync(string title, double precio);
        Task SendMessageAsyncDefaultCreden(string title, double precio);
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

        public async Task SendMessageAsyncDefaultCreden(string title, double precio)
        {
            ServiceBusClient client;
            ServiceBusSender sender;
            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            //TODO: Replace the "<NAMESPACE-NAME>" and "<QUEUE-NAME>" placeholders.
            client = new ServiceBusClient(
                _configuration["ServiceBus:NameSpaceQueue"],
                new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = Environment.GetEnvironmentVariable("client-id-queue") }),
                clientOptions);
            sender = client.CreateSender(_configuration["ServiceBus:QueueName"]);
            var messageBody = ($"Title: {title}, Precio: {precio}");

            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody))
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };
            IList<ServiceBusMessage> messages = new List<ServiceBusMessage>();
            messages.Add(message);
            await sender.SendMessagesAsync(messages);
        
        }
    }
}