using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using PaymentService.Application.Events;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace PaymentService.Infrastructure.Messaging
{
    public class DeliverySucceededConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DeliverySucceededConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("DeliverySucceededEvent", true, false, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine($"Delivery Succeeded: {json}");

                    var evt = JsonSerializer.Deserialize<DeliverySucceededEvent>(json);
                    if (evt == null) return;

                    if (evt.PaymentMethod != PaymentMethod.COD.ToString())
                        return;

                    using var scope = _scopeFactory.CreateScope();

                    var repo = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();

                    var payment = await repo.GetByOrderIdAsync(evt.OrderId);
                    if (payment == null || payment.Status != PaymentStatus.Pending)
                        return;

                    Console.WriteLine($"COD payment is still pending for order {evt.OrderId}. Shipper must confirm cash received via API.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DeliverySucceededConsumer error: " + ex.Message);
                }
            };

            channel.BasicConsume("DeliverySucceededEvent", true, consumer);
        }
    }
}
