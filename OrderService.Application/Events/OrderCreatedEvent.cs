using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
namespace OrderService.Application.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public string PaymentMethod { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
