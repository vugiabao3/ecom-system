using OrderService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;


namespace OrderService.Infrastructure.Messaging
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQEventBus()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "rabbitmq"
            };
        }

        public Task PublishAsync<T>(string queueName, T @event)
        {
            using var connection = _factory.CreateConnection(); // ✅ sync
            using var channel = connection.CreateModel();       // ✅ sync

            channel.QueueDeclare(queueName, true, false, false);

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );

            return Task.CompletedTask;
        }
    }
}
