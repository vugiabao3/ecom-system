using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using NotificationService.Domain.Events;
using NotificationService.Infrastructure.Services;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationService.Infrastructure.Messaging
{
    public class ShippingCreatedConsumer
    {
        private readonly NotificationServices _notificationService;
        private readonly IServiceScopeFactory _scopeFactory;

        public ShippingCreatedConsumer(
            NotificationServices notificationService,
            IServiceScopeFactory scopeFactory)
        {
            _notificationService = notificationService;
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

            channel.QueueDeclare(
                queue: "NotificationService.ShippingCreatedEvent",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            channel.QueueBind("NotificationService.ShippingCreatedEvent", "ecom.events", "ShippingCreatedEvent");
            Console.WriteLine("🔥 ShippingCreatedConsumer started...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                Console.WriteLine($"🔥 Shipping Created: {json}");

                var evt = JsonSerializer.Deserialize<ShippingCreatedEvent>(json);
                if (evt == null) return;
                if (string.IsNullOrWhiteSpace(evt.UserId)) { Console.WriteLine($"WARN: missing recipient UserId for {evt.GetType().Name} Order {evt.OrderId}"); return; }

                // 👉 Gửi notification cho khách hàng & người bán
                await _notificationService.SendShippingCreated(evt.OrderId, evt.ShipmentId);

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

var customerNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = evt.UserId,
                    Type = "ShippingCreated_Customer",
                    Title = "ShippingCreated_Customer",
                    Message = $"Your order {evt.OrderId} is being shipped (Shipment {evt.ShipmentId})",
                    CreatedAt = DateTime.UtcNow
                };

                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!await NotificationServices.ShouldCreateAsync(db, evt.UserId!, customerNotification.Message)) { Console.WriteLine("SKIP duplicate notification"); return; }
                await repo.AddAsync(customerNotification);
                await repo.SaveChangesAsync();
            };
            channel.BasicConsume(
                queue: "NotificationService.ShippingCreatedEvent",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}



