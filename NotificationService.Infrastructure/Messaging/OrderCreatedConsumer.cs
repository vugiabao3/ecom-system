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
using Microsoft.AspNetCore.SignalR;
using NotificationService.Infrastructure.Hubs;

namespace NotificationService.Infrastructure.Messaging;

public class OrderCreatedConsumer
{
    private readonly NotificationServices _notificationService;
    private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<NotificationsHub> _hubContext;

    public OrderCreatedConsumer(
        NotificationServices notificationService,
        IServiceScopeFactory scopeFactory,
            IHubContext<NotificationsHub> hubContext)
    {
        _notificationService = notificationService;
        _scopeFactory = scopeFactory;
            _hubContext = hubContext;
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
            queue: "NotificationService.OrderCreated",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
            channel.QueueBind("NotificationService.OrderCreated", "ecom.events", "OrderCreated");
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
                    Title = "OrderCreated",
                Message = $"Order {orderEvent.OrderId} has been created",
                CreatedAt = DateTime.UtcNow
            };

            await repo.AddAsync(notification);
            await repo.SaveChangesAsync();
            
            await _hubContext.Clients.User(orderEvent.UserId).SendAsync("ReceiveNotification", notification);
            
        };
        channel.BasicConsume(
            queue: "NotificationService.OrderCreated",
            autoAck: true,
            consumer: consumer
        );
    }
}


