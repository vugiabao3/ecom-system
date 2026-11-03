using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcomSystem.Contracts.Enums;

namespace OrderService.Domain.Entities
{
    public class CheckoutSession
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string? IdempotencyKey { get; set; }
        public CheckoutSessionStatus Status { get; set; } = CheckoutSessionStatus.Pending;
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string? CouponCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ReceiverName { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.QR;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public Guid? OrderId { get; set; }
        public Order Order { get; set; }
        public List<CheckoutSessionItem> Items { get; set; }
    }

    public class CheckoutSessionItem
    {
        public Guid Id { get; set; }
        public Guid CheckoutSessionId { get; set; }
        public CheckoutSession CheckoutSession { get; set; }
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string ProductName { get; set; }
        public string? Sku { get; set; }
        public string? VariantName { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
