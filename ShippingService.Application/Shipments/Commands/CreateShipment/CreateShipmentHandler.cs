using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShippingService.Application.Interfaces;
using ShippingService.Domain.Entities;
using ShippingService.Application.Events;
using EcomSystem.Contracts.Enums;

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
            var existing = await _repo.GetByOrderIdAsync(request.OrderId);
            if (existing != null)
            {
                return new CreateShipmentResponse
                {
                    ShipmentId = existing.Id,
                    Status = existing.Status.ToString()
                };
            }

            var shipment = new Shipment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                ShipperId = request.ShipperId,
                Address = request.Address,
                Phone = request.Phone,
                ReceiverName = request.ReceiverName,
                Status = ShipmentStatus.Created,
                TrackingCode = GenerateTrackingCode(),
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(shipment);
            await _repo.SaveChangesAsync();

            await _eventBus.PublishAsync(new ShippingCreatedEvent
            {
                OrderId = request.OrderId,
                ShipmentId = shipment.Id
            });

            return new CreateShipmentResponse
            {
                ShipmentId = shipment.Id,
                Status = shipment.Status.ToString()
            };
        }

        private string GenerateTrackingCode()
        {
            return $"TRK-{Guid.NewGuid().ToString("N")[..10].ToUpper()}";
        }
    }
}
