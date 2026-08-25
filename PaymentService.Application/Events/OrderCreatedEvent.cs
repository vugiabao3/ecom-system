using PaymentService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public string PaymentMethod { get; set; } = "QR";
    }
}
