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
            channel.ExchangeDeclare("ecom.events", "topic", true);

            var eventName = "ProductDeleted";
            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(@event)
            );

            channel.BasicPublish(
                exchange: "ecom.events",
                routingKey: eventName,
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"?? EVENT SENT TO RABBITMQ: {eventName}");

            return Task.CompletedTask;
        }
    }
}

