using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

using InventoryService.Application.Events;
using InventoryService.Application.Inventory.EventHandlers;

namespace InventoryService.Infrastructure.Messaging
{
    public class OrderCreatedConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderCreatedConsumer(IServiceScopeFactory scopeFactory)
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
            channel.ExchangeDeclare("ecom.events", "topic", true);

            channel.QueueDeclare("InventoryService.OrderCreated", true, false, false);
            channel.QueueBind("InventoryService.OrderCreated", "ecom.events", "OrderCreated");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var @event = JsonSerializer.Deserialize<OrderCreatedEvent>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true

                    });
                Console.WriteLine("?? RAW JSON: " + json);
                if (@event == null || @event.Items == null)
                {
                    Console.WriteLine("? Event ho?c Items null");
                    return;
                }

                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider
                    .GetRequiredService<OrderCreatedEventHandler>();

                await handler.Handle(@event);
            };

            channel.BasicConsume("InventoryService.OrderCreated", true, consumer);
        }
    }
}
