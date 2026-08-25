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

namespace NotificationService.Infrastructure.Messaging;

public class OrderCreatedConsumer
{
    private readonly NotificationServices _notificationService;
    private readonly IServiceScopeFactory _scopeFactory;

    public OrderCreatedConsumer(
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
            queue: "OrderCreated",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        Console.WriteLine("?? OrderCreatedConsumer started...");

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            Console.WriteLine($"?? Received message: {json}");

            var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(json);
            if (orderEvent == null)
            {
                Console.WriteLine("? Deserialize failed");
                return;
            }
            Console.WriteLine($"? OrderId: {orderEvent.OrderId}");

            // ?? G?i notification cho khách hàng
            await _notificationService.SendOrderCreated(orderEvent.OrderId);

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = orderEvent.UserId,
                Type = "OrderCreated",
                Message = $"Order {orderEvent.OrderId} has been created",
                CreatedAt = DateTime.UtcNow
            };

            await repo.AddAsync(notification);
            await repo.SaveChangesAsync();
        };
        channel.BasicConsume(
            queue: "OrderCreated",
            autoAck: true,
            consumer: consumer
        );
    }
}
