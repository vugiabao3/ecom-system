using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Application.Events
{
    public class ReturnOrderEvent
    {
        public Guid OrderId { get; set; }
        public Guid ShipmentId { get; set; }
        public string? Reason { get; set; }
        public List<DTOs.OrderItemDto> Items { get; set; }
    }
}
