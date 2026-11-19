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
    public class ReturnOrderConsumer
    {
        private readonly NotificationServices _notificationService;
        private readonly IServiceScopeFactory _scopeFactory;

        public ReturnOrderConsumer(
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
                queue: "NotificationService.ReturnOrderEvent",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            channel.QueueBind("NotificationService.ReturnOrderEvent", "ecom.events", "ReturnOrderEvent");
            Console.WriteLine("🔥 ReturnOrderConsumer started...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                Console.WriteLine($"🔥 Return Order: {json}");

var evt = JsonSerializer.Deserialize<ReturnOrderEvent>(json);
                if (evt == null) return;
                if (string.IsNullOrWhiteSpace(evt.UserId)) { Console.WriteLine($"WARN: missing recipient UserId for ReturnOrder Order {evt.OrderId}"); return; }

                // Send notification to the customer
                await _notificationService.SendReturnOrder(evt.OrderId, evt.Reason ?? "Unknown");

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var customerNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = evt.UserId,
                    Type = "ReturnOrder_Customer",
                    Title = "ReturnOrder_Customer",
                    Message = $"Your order {evt.OrderId} has been returned. Reason: {evt.Reason}",
                    CreatedAt = DateTime.UtcNow
                };

                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!await NotificationServices.ShouldCreateAsync(db, evt.UserId!, customerNotification.Message)) { Console.WriteLine("SKIP duplicate notification"); return; }
                await repo.AddAsync(customerNotification);
                await repo.SaveChangesAsync();
            };
            channel.BasicConsume(
                queue: "NotificationService.ReturnOrderEvent",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}



