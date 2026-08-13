using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Events;
using MediatR;
using PaymentService.Application.Payments.Commands.ProcessPayment;
namespace PaymentService.Infrastructure.Messaging
{
    public class OrderCreatedConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderCreatedConsumer(IServiceScopeFactory scopeFactory)
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

            channel.QueueDeclare("OrderCreated", true, false, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                    var @event = JsonSerializer.Deserialize<OrderCreatedEvent>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    if (@event == null)
                    {
                        Console.WriteLine("❌ Event null");
                        return;
                    }

                    using var scope = _scopeFactory.CreateScope();

                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Send(new ProcessPaymentCommand
                    {
                        OrderId = @event.OrderId
                        //Amount = @event.TotalAmount,
                        //Items = @event.Items
                    });
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    // 🔥 QUAN TRỌNG: KHÔNG ĐƯỢC CRASH APP
                    Console.WriteLine("❌ Consumer error: " + ex.Message);

                    // optional: log DB / retry queue
                }
            };

            channel.BasicConsume("OrderCreated", autoAck: false, consumer);

        }
    }
}
