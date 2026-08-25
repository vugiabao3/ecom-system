using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using InventoryService.Application.Events;
using InventoryService.Application.Inventory.EventHandlers;

namespace InventoryService.Infrastructure.Messaging
{
    public class OrderReturnedConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderReturnedConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("OrderReturned", true, false, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var @event = JsonSerializer.Deserialize<OrderReturnedEvent>(json);
                if (@event == null) return;

                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<OrderReturnedEventHandler>();
                await handler.Handle(@event);
            };

            channel.BasicConsume("OrderReturned", true, consumer);
        }
    }
}
