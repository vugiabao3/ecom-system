using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ProductService.Infrastructure.Messaging
{
    public class RabbitMQEventBus : IEventBus
    {
        public Task PublishAsync<T>(T @event)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var eventName = "ProductDeleted";
            channel.QueueDeclare(
                queue: eventName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(@event)
            );

            channel.BasicPublish(
                exchange: "",
                routingKey: eventName,
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"🔥 EVENT SENT TO RABBITMQ: {eventName}");

            return Task.CompletedTask;
        }
    }
}
