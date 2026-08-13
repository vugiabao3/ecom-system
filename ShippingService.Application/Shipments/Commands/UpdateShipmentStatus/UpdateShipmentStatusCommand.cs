using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace ShippingService.Application.Shipments.Commands.UpdateShipmentStatus
{
    public class UpdateShipmentStatusCommand : IRequest<bool>
    {
        public Guid ShipmentId { get; set; }
        public string Status { get; set; }
    }
}