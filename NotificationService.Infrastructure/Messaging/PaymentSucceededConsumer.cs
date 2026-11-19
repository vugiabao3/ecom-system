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
using NotificationService.Domain.Interfaces;
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
            channel.ExchangeDeclare("ecom.events", "topic", true);

            channel.QueueDeclare(
                queue: "NotificationService.PaymentSucceededEvent",
                durable: true,
                exclusive: false,
                autoDelete: false
            );
            channel.QueueBind("NotificationService.PaymentSucceededEvent", "ecom.events", "PaymentSucceededEvent");

            Console.WriteLine("?? PaymentSucceededConsumer started...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                Console.WriteLine($"?? Received Payment Event: {json}");

                var evt = JsonSerializer.Deserialize<PaymentSucceededEvent>(json);

                if (evt == null) return;
                if (string.IsNullOrWhiteSpace(evt.UserId)) { Console.WriteLine($"WARN: missing recipient UserId for {evt.GetType().Name} Order {evt.OrderId}"); return; }
                Console.WriteLine($"?? Payment success for order: {evt.OrderId}");

                // ?? G?i notification cho khách hàng
                await _notificationService.SendPaymentSuccess(evt.OrderId);

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = evt.UserId,
                    Type = "PaymentSucceeded",
                    Title = "PaymentSucceeded",
                    Message = $"Payment success for Order {evt.OrderId}",
                    CreatedAt = DateTime.UtcNow
                };

                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!await NotificationServices.ShouldCreateAsync(db, evt.UserId!, notification.Message)) { Console.WriteLine("SKIP duplicate notification"); return; }
                await repo.AddAsync(notification);
                await repo.SaveChangesAsync();
            };
            channel.BasicConsume(
                queue: "NotificationService.PaymentSucceededEvent",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}



