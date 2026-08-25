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

namespace ShippingService.Application.Shipments.Commands.UpdateShipmentStatus
{
    public class UpdateShipmentStatusHandler
        : IRequestHandler<UpdateShipmentStatusCommand, bool>
    {
        private readonly IShipmentRepository _repo;
        private readonly IEventBus _eventBus;
        private readonly IOrderServiceClient _orderClient;

        public UpdateShipmentStatusHandler(IShipmentRepository repo, IEventBus eventBus, IOrderServiceClient orderClient)
        {
            _repo = repo;
            _eventBus = eventBus;
            _orderClient = orderClient;
        }

        public async Task<bool> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
        {
            var shipment = await _repo.GetByIdAsync(request.ShipmentId);

            if (shipment == null)
                return false;

            if (request.Status == ShipmentStatus.Failed && string.IsNullOrWhiteSpace(request.FailureReason))
                throw new Exception("Failure reason is required for failed status");

            var previousStatus = shipment.Status;

            if (!IsValidTransition(previousStatus, request.Status))
                throw new Exception($"Invalid status transition from {previousStatus} to {request.Status}");

            shipment.Status = request.Status;
            shipment.UpdatedAt = DateTime.UtcNow;

            if (request.Status == ShipmentStatus.Failed)
            {
                shipment.FailureReason = request.FailureReason;
            }

            if (request.Status == ShipmentStatus.Delivered)
            {
                shipment.DeliveredAt = DateTime.UtcNow;
            }

            _repo.Update(shipment);
            await _repo.SaveChangesAsync();

            if (request.Status == ShipmentStatus.Delivered)
            {
                var order = await _orderClient.GetOrder(shipment.OrderId);
                await _eventBus.PublishAsync(new DeliverySucceededEvent
                {
                    OrderId = shipment.OrderId,
                    ShipmentId = shipment.Id,
                    PaymentMethod = order?.PaymentMethod ?? "QR"
                });
            }

            if (request.Status == ShipmentStatus.Failed)
            {
                var order = await _orderClient.GetOrder(shipment.OrderId);
                await _eventBus.PublishAsync(new DeliveryFailedEvent
                {
                    OrderId = shipment.OrderId,
                    ShipmentId = shipment.Id,
                    Reason = request.FailureReason
                });
                await _eventBus.PublishAsync(new ReturnOrderEvent
                {
                    OrderId = shipment.OrderId,
                    ShipmentId = shipment.Id,
                    Reason = request.FailureReason,
                    Items = order?.Items ?? new List<ShippingService.Application.DTOs.OrderItemDto>()
                });
            }

            return true;
        }

        private bool IsValidTransition(ShipmentStatus from, ShipmentStatus to)
        {
            return (from, to) switch
            {
                (ShipmentStatus.Created, ShipmentStatus.Assigned) => true,
                (ShipmentStatus.Assigned, ShipmentStatus.PickedUp) => true,
                (ShipmentStatus.PickedUp, ShipmentStatus.Delivering) => true,
                (ShipmentStatus.Delivering, ShipmentStatus.Delivered) => true,
                (ShipmentStatus.Delivering, ShipmentStatus.Failed) => true,
                (ShipmentStatus.Failed, ShipmentStatus.Returned) => true,
                _ => false
            };
        }
    }
}
