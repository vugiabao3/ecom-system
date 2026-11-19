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
    public class DeliverySucceededConsumer
    {
        private readonly NotificationServices _notificationService;
        private readonly IServiceScopeFactory _scopeFactory;

        public DeliverySucceededConsumer(
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
                queue: "NotificationService.DeliverySucceededEvent",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            channel.QueueBind("NotificationService.DeliverySucceededEvent", "ecom.events", "DeliverySucceededEvent");
            Console.WriteLine("🔥 DeliverySucceededConsumer started...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                Console.WriteLine($"🔥 Delivery Succeeded: {json}");

var evt = JsonSerializer.Deserialize<DeliverySucceededEvent>(json);
                if (evt == null) return;
                if (string.IsNullOrWhiteSpace(evt.UserId)) { Console.WriteLine($"WARN: missing recipient UserId for DeliverySucceeded Order {evt.OrderId}"); return; }

                // Send notification to the customer
                await _notificationService.SendDeliverySuccess(evt.OrderId);

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var customerNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = evt.UserId,
                    Type = "DeliverySucceeded_Customer",
                    Title = "DeliverySucceeded_Customer",
                    Message = $"Your order {evt.OrderId} has been delivered successfully",
                    CreatedAt = DateTime.UtcNow
                };

                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!await NotificationServices.ShouldCreateAsync(db, evt.UserId!, customerNotification.Message)) { Console.WriteLine("SKIP duplicate notification"); return; }
                await repo.AddAsync(customerNotification);
                await repo.SaveChangesAsync();
            };
            channel.BasicConsume(
                queue: "NotificationService.DeliverySucceededEvent",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}



