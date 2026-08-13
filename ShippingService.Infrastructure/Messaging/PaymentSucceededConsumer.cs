using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShippingService.Application.Events;
using ShippingService.Application.Interfaces;
using ShippingService.Application.Shipments.Commands.CreateShipment;
using System.Text;
using System.Text.Json;

namespace ShippingService.Infrastructure.Messaging
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
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "PaymentSucceededEvent",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            Console.WriteLine("🚚 ShippingConsumer STARTED, waiting message...");

            var consumer = new EventingBasicConsumer(channel);
            Console.WriteLine("🔥 BEFORE RECEIVED");
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    Console.WriteLine("📩 RECEIVED EVENT");

                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                    Console.WriteLine(json);

                    var evt = JsonSerializer.Deserialize<PaymentSucceededEvent>(json);

                    if (evt == null)
                    {
                        Console.WriteLine("❌ Deserialize FAILED");
                        return;
                    }

                    Console.WriteLine($"✅ Parsed OrderId: {evt.OrderId}");

                    using var scope = _scopeFactory.CreateScope();

                    var mediator = scope.ServiceProvider
                        .GetRequiredService<IMediator>();

                    var orderClient = scope.ServiceProvider
                        .GetRequiredService<IOrderServiceClient>();

                    Console.WriteLine("🚀 CALL ORDER SERVICE...");

                    var order = await orderClient.GetOrder(evt.OrderId);

                    if (order == null)
                    {
                        Console.WriteLine("❌ ORDER IS NULL");
                        return;
                    }

                    Console.WriteLine("📦 ORDER DATA:");
                    Console.WriteLine($"Address: {order.Address}");
                    Console.WriteLine($"Phone: {order.Phone}");
                    Console.WriteLine($"Receiver: {order.ReceiverName}");

                    await mediator.Send(new CreateShipmentCommand
                    {
                        OrderId = evt.OrderId,
                        Address = order.Address,
                        Phone = order.Phone,
                        ReceiverName = order.ReceiverName
                    });

                    Console.WriteLine("✅ SHIPMENT CREATED SUCCESSFULLY");

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ CONSUMER ERROR");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            };
            Console.WriteLine("🔥 BEFORE BASIC CONSUME");

            // 🔥 CỰC QUAN TRỌNG
            channel.BasicConsume(
                queue: "PaymentSucceededEvent",
                autoAck: false,
                consumer: consumer
            );
            Console.WriteLine("🔥 AFTER BASIC CONSUME");
        }
    }
}