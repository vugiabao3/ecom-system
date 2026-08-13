using PromotionService.Application.Events;
using PromotionService.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace PromotionService.Infrastructure.Messaging
{
    public class PaymentSucceededConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private IConnection _connection;
        private IModel _channel;

        public PaymentSucceededConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {

            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "PaymentSucceededEvent",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await HandleMessage(message);
            };

            _channel.BasicConsume(
                queue: "PaymentSucceededEvent",
                autoAck: true,
                consumer: consumer
            );

            Console.WriteLine("🔥 Promotion Consumer STARTED");
        }

        private async Task HandleMessage(string message)
        {
            var evt = JsonSerializer.Deserialize<PaymentSucceededEvent>(message);

            if (evt == null) return;

            Console.WriteLine($"💳 Payment received for user {evt.UserId}");

            // 🔥 tạo scope mỗi message
            using var scope = _scopeFactory.CreateScope();

            var repo = scope.ServiceProvider.GetRequiredService<IUserPointRepository>();

            var userPoint = await repo.GetByUserId(evt.UserId);

            if (userPoint == null)
            {
                userPoint = new Domain.Entities.UserPoint
                {
                    Id = Guid.NewGuid(),
                    UserId = evt.UserId,
                    Points = 0
                };
                await repo.AddAsync(userPoint); // 🔥 THÊM DÒNG NÀY

            }

            else
            {
                userPoint.Points += (int)(evt.TotalAmount / 1000);
            }

            await repo.SaveChangesAsync();
            Console.WriteLine("RAW MESSAGE:");
            Console.WriteLine(message);

            Console.WriteLine("USER ID:");
            Console.WriteLine(evt?.UserId);
            Console.WriteLine($"✅ Updated points: {userPoint.Points}");
        }
    }
}