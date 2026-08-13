using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using InventoryService.Application.Events;
using InventoryService.Application.Inventory.EventHandlers;

namespace InventoryService.Infrastructure.Messaging
{
    public class PaymentSucceededConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentSucceededConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("PaymentSucceededEvent", true, false, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var @event = JsonSerializer.Deserialize<PaymentSucceededEvent>(json);

                if (@event == null) return;

                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider
                    .GetRequiredService<PaymentSucceededEventHandler>();

                await handler.Handle(@event);
            };

            channel.BasicConsume("PaymentSucceededEvent", true, consumer);
        }
    }
}