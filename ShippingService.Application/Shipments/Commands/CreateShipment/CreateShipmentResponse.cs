using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Application.Shipments.Commands.CreateShipment
{
    public class CreateShipmentResponse
    {
        public Guid ShipmentId { get; set; }
        public string Status { get; set; }
    }
}