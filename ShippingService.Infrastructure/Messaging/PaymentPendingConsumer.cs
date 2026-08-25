using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using ShippingService.Application.Events;
using ShippingService.Application.Shipments.Commands.CreateShipment;
using ShippingService.Application.Interfaces;
using EcomSystem.Contracts.Enums;

namespace ShippingService.Infrastructure.Messaging
{
    public class PaymentPendingConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentPendingConsumer(IServiceScopeFactory scopeFactory)
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

            channel.QueueDeclare("PaymentPendingEvent", true, false, false);

            Console.WriteLine("🔥 PaymentPendingConsumer started...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"🔥 Payment Pending Event: {json}");

                var evt = JsonSerializer.Deserialize<PaymentPendingEvent>(json);
                if (evt == null) return;

                if (evt.PaymentMethod != PaymentMethod.COD.ToString())
                    return;

                using var scope = _scopeFactory.CreateScope();

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var orderClient = scope.ServiceProvider.GetRequiredService<IOrderServiceClient>();

                var order = await orderClient.GetOrder(evt.OrderId);
                if (order == null) return;

                await mediator.Send(new CreateShipmentCommand
                {
                    OrderId = evt.OrderId,
                    Address = order.Address,
                    ReceiverName = order.ReceiverName,
                    Phone = order.Phone
                });

                Console.WriteLine($"📦 COD shipment created for order {evt.OrderId}");
            };

            channel.BasicConsume("PaymentPendingEvent", true, consumer);
        }
    }
}
