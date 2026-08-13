//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using MediatR;
//using Microsoft.Extensions.DependencyInjection;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using ShippingService.Application.Events;
//using ShippingService.Application.Shipments.Commands.UpdateShipmentStatus;
//using System.Text.Json;

//namespace ShippingService.Infrastructure.Messaging
//{
//    public class ShippingCreatedConsumer
//    {
//        private readonly IServiceScopeFactory _scopeFactory;

//        public ShippingCreatedConsumer(IServiceScopeFactory scopeFactory)
//        {
//            _scopeFactory = scopeFactory;
//        }

//        public void Start()
//        {
//            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
//            var connection = factory.CreateConnection();
//            var channel = connection.CreateModel();

//            channel.QueueDeclare("ShippingCreatedEvent", false, false, false);

//            Console.WriteLine("📦 ShippingCreatedConsumer STARTED...");

//            var consumer = new EventingBasicConsumer(channel);

//            consumer.Received += async (model, ea) =>
//            {
//                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
//                Console.WriteLine($"📩 RECEIVED ShippingCreated: {json}");

//                var evt = JsonSerializer.Deserialize<ShippingCreatedEvent>(json);

//                if (evt == null) return;

//                using var scope = _scopeFactory.CreateScope();

//                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

//                await mediator.Send(new UpdateShipmentStatusCommand
//                {
//                    ShipmentId = evt.ShipmentId,
//                    Status = "DELIVERING"
//                });

//                Console.WriteLine("🚚 Status updated to DELIVERING");
//            };

//            channel.BasicConsume("ShippingCreatedEvent", true, consumer);
//        }
//    }
//}