using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShippingService.Application.DTOs;

namespace ShippingService.Application.Shipments.Queries.GetShipmentsByShipperId
{
    public class GetShipmentsByShipperIdQuery : IRequest<List<ShipmentWithOrderDto>>
    {
        public Guid ShipperId { get; set; }
    }
}
