using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Application.DTOs
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderDto
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string ReceiverName { get; set; }
        public string Phone { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
