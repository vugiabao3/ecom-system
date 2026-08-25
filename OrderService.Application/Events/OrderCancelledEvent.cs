using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;

namespace OrderService.Application.Events
{
    public class OrderCancelledEvent
    {
        public Guid OrderId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
