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

namespace ShippingService.Application.Shipments.Commands.AssignShipment
{
    public class AssignShipmentHandler : IRequestHandler<AssignShipmentCommand, bool>
    {
        private readonly IShipmentRepository _repo;
        private readonly IEventBus _eventBus;

        public AssignShipmentHandler(IShipmentRepository repo, IEventBus eventBus)
        {
            _repo = repo;
            _eventBus = eventBus;
        }

        public async Task<bool> Handle(AssignShipmentCommand request, CancellationToken cancellationToken)
        {
            var shipment = await _repo.GetByIdAsync(request.ShipmentId);

            if (shipment == null)
                return false;

            if (shipment.Status != ShipmentStatus.Created)
                return false;

            shipment.ShipperId = request.ShipperId;
            shipment.Status = ShipmentStatus.Assigned;
            shipment.UpdatedAt = DateTime.UtcNow;

            _repo.Update(shipment);
            await _repo.SaveChangesAsync();

            await _eventBus.PublishAsync(new ShippingCreatedEvent
            {
                OrderId = shipment.OrderId,
                ShipmentId = shipment.Id
            });

            return true;
        }
    }
}
