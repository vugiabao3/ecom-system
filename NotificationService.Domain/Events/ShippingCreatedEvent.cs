using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain.Events
{
    public class ShippingCreatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid ShipmentId { get; set; }
    }
}
