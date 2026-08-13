using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ShippingService.Application.Interfaces;

namespace ShippingService.Infrastructure.Messaging
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IConnection _connection;
        private const string QueueName = "PaymentSucceededEvent";

        public RabbitMqEventBus()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();

            Console.WriteLine("🔥 RabbitMQ CONNECTED");
        }

        public Task PublishAsync<T>(T @event)
        {
            if (!_connection.IsOpen)
                throw new Exception("RabbitMQ connection is closed");

            using var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var json = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            Console.WriteLine("🚀 PUBLISHING EVENT -> " + QueueName);
            Console.WriteLine(json);

            channel.BasicPublish(
                exchange: "",
                routingKey: QueueName,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine("✅ EVENT PUBLISHED SUCCESSFULLY");

            return Task.CompletedTask;
        }
    }
}