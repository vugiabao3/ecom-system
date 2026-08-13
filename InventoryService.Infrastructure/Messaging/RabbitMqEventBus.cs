using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RabbitMQ.Client;
using System.Text.Json;
using InventoryService.Application.Interfaces;

namespace InventoryService.Infrastructure.Messaging
{
    public class RabbitMqEventBus : IEventBus
    {
        public Task PublishAsync<T>(T @event)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var json = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish("", typeof(T).Name, null, body);

            return Task.CompletedTask;
        }
    }
}
