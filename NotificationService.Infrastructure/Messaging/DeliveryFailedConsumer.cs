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
    public class DeliveryFailedConsumer
    {
        private readonly NotificationServices _notificationService;
        private readonly IServiceScopeFactory _scopeFactory;

        public DeliveryFailedConsumer(
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
                queue: "NotificationService.DeliveryFailedEvent",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            channel.QueueBind("NotificationService.DeliveryFailedEvent", "ecom.events", "DeliveryFailedEvent");
            Console.WriteLine("🔥 DeliveryFailedConsumer started...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                Console.WriteLine($"🔥 Delivery Failed: {json}");

var evt = JsonSerializer.Deserialize<DeliveryFailedEvent>(json);
                if (evt == null) return;
                if (string.IsNullOrWhiteSpace(evt.UserId)) { Console.WriteLine($"WARN: missing recipient UserId for DeliveryFailed Order {evt.OrderId}"); return; }

                // Send notification to the customer
                await _notificationService.SendDeliveryFailed(evt.OrderId, evt.Reason ?? "Unknown");

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var customerNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = evt.UserId,
                    Type = "DeliveryFailed_Customer",
                    Title = "DeliveryFailed_Customer",
                    Message = $"Delivery failed for Order {evt.OrderId}: {evt.Reason}",
                    CreatedAt = DateTime.UtcNow
                };

                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!await NotificationServices.ShouldCreateAsync(db, evt.UserId!, customerNotification.Message)) { Console.WriteLine("SKIP duplicate notification"); return; }
                await repo.AddAsync(customerNotification);
                await repo.SaveChangesAsync();
            };
            channel.BasicConsume(
                queue: "NotificationService.DeliveryFailedEvent",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}



