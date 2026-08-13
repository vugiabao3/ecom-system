using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using CartService.Application.Events;
using CartService.Application.Cart.EventHandlers;

namespace CartService.Infrastructure.Events
{
    public class ProductDeletedConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ProductDeletedConsumer(IServiceScopeFactory scopeFactory)
        {
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
                queue: "ProductDeleted",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    var @event = JsonSerializer.Deserialize<ProductDeletedEvent>(json);

                    if (@event == null)
                        return;

                    using var scope = _scopeFactory.CreateScope();

                    var handler = scope.ServiceProvider
                        .GetRequiredService<ProductDeletedEventHandler>();

                    await handler.Handle(@event);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Consumer error: {ex.Message}");

                    // optional: reject message
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            channel.BasicConsume(
                queue: "ProductDeleted",
                autoAck: false,
                consumer: consumer
            );
        }
    }
}