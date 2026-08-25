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

            channel.QueueDeclare(
                queue: "DeliverySucceededEvent",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            Console.WriteLine("🔥 DeliverySucceededConsumer started...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                Console.WriteLine($"🔥 Delivery Succeeded: {json}");

                var evt = JsonSerializer.Deserialize<DeliverySucceededEvent>(json);
                if (evt == null) return;

                // 👉 Gửi notification cho khách hàng, người bán & admin
                await _notificationService.SendDeliverySuccess(evt.OrderId);

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var customerNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = null,
                    Type = "DeliverySucceeded_Customer",
                    Message = $"Your order {evt.OrderId} has been delivered successfully",
                    CreatedAt = DateTime.UtcNow
                };

                var sellerNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = null,
                    Type = "DeliverySucceeded_Seller",
                    Message = $"Order {evt.OrderId} has been delivered successfully",
                    CreatedAt = DateTime.UtcNow
                };

                var adminNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = null,
                    Type = "DeliverySucceeded_Admin",
                    Message = $"Order {evt.OrderId} delivered (payment method: {evt.PaymentMethod})",
                    CreatedAt = DateTime.UtcNow
                };

                await repo.AddAsync(customerNotification);
                await repo.AddAsync(sellerNotification);
                await repo.AddAsync(adminNotification);
                await repo.SaveChangesAsync();
            };
            channel.BasicConsume(
                queue: "DeliverySucceededEvent",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}
