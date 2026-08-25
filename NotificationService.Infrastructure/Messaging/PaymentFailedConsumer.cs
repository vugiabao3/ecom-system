using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using NotificationService.Domain.Events;

using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Services;
using NotificationService.Domain.Interfaces;

namespace NotificationService.Infrastructure.Messaging
{
    public class PaymentFailedConsumer
    {
        private readonly NotificationServices _notificationService;
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentFailedConsumer(
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
                queue: "PaymentFailedEvent",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            Console.WriteLine("?? PaymentFailedConsumer started...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                Console.WriteLine($"?? PAYMENT FAILED EVENT: {json}");

                var evt = JsonSerializer.Deserialize<PaymentFailedEvent>(json);

                if (evt == null) return;

                // ?? G?i notification cho khách hàng
                await _notificationService.SendPaymentFailed(evt.OrderId);

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = null,
                    Type = "PaymentFailed",
                    Message = $"Payment failed for Order {evt.OrderId}",
                    CreatedAt = DateTime.UtcNow
                };

                await repo.AddAsync(notification);
                await repo.SaveChangesAsync();
            };

                channel.BasicConsume(
                queue: "PaymentFailedEvent",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}
