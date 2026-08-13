using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using OrderService.Application.Events;
using OrderService.Application.Orders.EventHandlers;

namespace OrderService.Infrastructure.Messaging
{
    public class PaymentConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // 🔥 queue success
            channel.QueueDeclare("PaymentSucceededEvent", true, false, false);

            var consumer1 = new EventingBasicConsumer(channel);

            consumer1.Received += async (model, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var @event = JsonSerializer.Deserialize<PaymentSucceededEvent>(json);

                if (@event == null) return;

                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider
                    .GetRequiredService<PaymentSucceededEventHandler>();

                await handler.Handle(@event);
            };
                        channel.BasicConsume("PaymentSucceededEvent", true, consumer1);

            // 🔥 queue failed
            channel.QueueDeclare("PaymentFailedEvent", true, false, false);

            var consumer2 = new EventingBasicConsumer(channel);

            consumer2.Received += async (model, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var @event = JsonSerializer.Deserialize<PaymentFailedEvent>(json);

                if (@event == null) return;

                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider
                    .GetRequiredService<PaymentFailedEventHandler>();

                await handler.Handle(@event);
            };

            channel.BasicConsume("PaymentFailedEvent", true, consumer2);
        }
    }
}
