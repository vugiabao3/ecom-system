using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using NotificationService.Domain.Events;
using NotificationService.Infrastructure.Services;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;


namespace NotificationService.Infrastructure.Messaging
{
    public class PaymentSucceededConsumer
    {
        private readonly NotificationServices _notificationService;
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentSucceededConsumer(
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
                queue: "PaymentSucceededEvent",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            Console.WriteLine("🔥 PaymentSucceededConsumer started...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                Console.WriteLine($"🔥 Received Payment Event: {json}");

                var evt = JsonSerializer.Deserialize<PaymentSucceededEvent>(json);

                if (evt == null) return;
                Console.WriteLine($"💳 Payment success for order: {evt.OrderId}");

                await _notificationService.SendPaymentSuccess(evt.OrderId);


                // 💾 2. TẠO SCOPE ĐỂ LẤY DB
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = null,
                    Type = "PaymentSucceeded",
                    Message = $"Payment success for Order {evt.OrderId}",
                    CreatedAt = DateTime.UtcNow
                };

                db.Notifications.Add(notification);
                await db.SaveChangesAsync();
            };
            channel.BasicConsume(
                queue: "PaymentSucceededEvent",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}