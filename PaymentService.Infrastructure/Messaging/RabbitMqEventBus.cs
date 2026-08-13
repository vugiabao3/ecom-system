using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using PaymentService.Application.Interfaces;

namespace PaymentService.Infrastructure.Messaging
{
    public class RabbitMqEventBus : IEventBus
    {
        public async Task PublishAsync<T>(T @event)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var queue = "PaymentSucceededEvent"; // 🔥 FIX cứng theo consumer


            // ✔ FIX 1: durable = true
            channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false
            );
            var json = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(json);

            // ✔ FIX 2: persistent message
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            Console.WriteLine($"🔥 PUBLISH TO QUEUE: {queue}");
            channel.BasicPublish(
                exchange: "",
                routingKey: queue,
                basicProperties: properties,
                body: body
            );


            await Task.CompletedTask;
        }
    }
}