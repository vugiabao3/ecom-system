using InventoryService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Application.Events
{
    public class PaymentFailedEvent
    {
        public Guid OrderId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
