using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using RabbitMQ.Client.Events;
using PaymentService.Application.Events;
using CartService.Application.Cart.EventHandlers;

namespace CartService.Infrastructure.Events
{
    public class PaymentSucceededConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentSucceededConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "PaymentSucceeded",
                durable: true,
                exclusive: false,
                autoDelete: false);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var @event = JsonSerializer.Deserialize<PaymentSucceededEvent>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                if (@event == null)
                    return;
                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider
                    .GetRequiredService<PaymentSucceededEventHandler>();

                await handler.Handle(@event);
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume("PaymentSucceeded", false, consumer);
        }
    }
}
