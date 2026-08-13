using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ShippingService.Application.Shipments.Commands.CreateShipment
{
    public class CreateShipmentCommand : IRequest<CreateShipmentResponse>
    {
        public Guid OrderId { get; set; }
        public string Address { get; set; }
        public string ReceiverName { get; set; }
        public string Phone { get; set; }
    }
}