using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Events
{
    public class DeliveryFailedEvent
    {
        public Guid OrderId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
