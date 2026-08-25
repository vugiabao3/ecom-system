using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcomSystem.Contracts.Enums;

namespace OrderService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Phone { get; set; }
        public string ReceiverName { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.QR;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public List<OrderItem> Items { get; set; }
        public List<OrderStatusHistory> StatusHistory { get; set; }
    }
}
