using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
namespace OrderService.Application.Events
{
    namespace OrderService.Application.Events
    {
        public class OrderCreatedEvent
        {
            public Guid OrderId { get; set; }
            public List<OrderItemDto> Items { get; set; }
            public decimal TotalAmount { get; set; } // 🔥 thêm

            public string UserId { get; set; } // 🔥 thêm
        }

      
    }
}