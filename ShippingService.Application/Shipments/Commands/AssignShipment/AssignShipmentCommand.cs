using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcomSystem.Contracts.Enums;

namespace ShippingService.Application.Shipments.Commands.AssignShipment
{
    public class AssignShipmentCommand : IRequest<bool>
    {
        public Guid ShipmentId { get; set; }
        public Guid ShipperId { get; set; }
    }
}
