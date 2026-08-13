using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using ShippingService.Application.Interfaces;
using ShippingService.Domain.Entities;
using ShippingService.Application.Events;

namespace ShippingService.Application.Shipments.Commands.CreateShipment
{
    public class CreateShipmentHandler : IRequestHandler<CreateShipmentCommand, CreateShipmentResponse>
    {
        private readonly IShipmentRepository _repo;
        private readonly IEventBus _eventBus;

        public CreateShipmentHandler(IShipmentRepository repo, IEventBus eventBus)
        {
            _repo = repo;
            _eventBus = eventBus;
        }

        public async Task<CreateShipmentResponse> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("🔥 CreateShipmentHandler CALLED");

            var shipment = new Shipment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Address = request.Address,
                Phone = request.Phone,              // ✅ FIX
                ReceiverName = request.ReceiverName, // ✅ FIX
                Status = "CREATED",
                TrackingCode = GenerateTrackingCode(), // 🔥 FIX QUAN TRỌNG
                CreatedAt = DateTime.UtcNow
            };
            Console.WriteLine($"🧾 Creating shipment for OrderId: {request.OrderId}");

            await _repo.AddAsync(shipment);
            await _repo.SaveChangesAsync();
            Console.WriteLine("💾 Saved to DATABASE");

            // 🔥 publish event
            await _eventBus.PublishAsync(new ShippingCreatedEvent
            {
                OrderId = request.OrderId,
                ShipmentId = shipment.Id
            });
            Console.WriteLine("📡 ShippingCreatedEvent published");


            return new CreateShipmentResponse
            {
                ShipmentId = shipment.Id,
                Status = shipment.Status
            };

        }
        // 🔥 ADD METHOD NÀY
        private string GenerateTrackingCode()
        {
            return $"TRK-{Guid.NewGuid().ToString("N")[..10].ToUpper()}";
        }
    }
}