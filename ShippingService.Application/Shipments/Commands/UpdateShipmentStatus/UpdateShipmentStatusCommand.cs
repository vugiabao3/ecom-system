using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using EcomSystem.Contracts.Enums;

namespace ShippingService.Application.Shipments.Commands.UpdateShipmentStatus
{
    public class UpdateShipmentStatusCommand : IRequest<bool>
    {
        public Guid ShipmentId { get; set; }
        public ShipmentStatus Status { get; set; }
        public string? FailureReason { get; set; }
    }
}
