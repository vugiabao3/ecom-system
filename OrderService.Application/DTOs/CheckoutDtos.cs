using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DTOs
{
    public class CheckoutRequest
    {
        public Guid? CheckoutId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ReceiverName { get; set; }
        public string? CouponCode { get; set; }
        public string PaymentMethod { get; set; } = "QR";
        public string? IdempotencyKey { get; set; }
    }

    public class CheckoutResponse
    {
        public Guid OrderId { get; set; }
        public Guid? CheckoutId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }

    public class PreviewRequest
    {
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? ReceiverName { get; set; }
        public string? CouponCode { get; set; }
        public string PaymentMethod { get; set; } = "QR";
    }

    public class PreviewItemDto
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string ProductName { get; set; }
        public string? Sku { get; set; }
        public string? VariantName { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Price * Quantity;
        public bool IsValid { get; set; }
        public string? Error { get; set; }
    }

    public class PreviewResponse
    {
        public Guid CheckoutId { get; set; }
        public List<PreviewItemDto> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class ValidateRequest
    {
        public string? CouponCode { get; set; }
    }

    public class ItemValidationDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public bool IsValid { get; set; }
        public string? Error { get; set; }
    }

    public class ValidateResponse
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<ItemValidationDto> ItemValidations { get; set; } = new();
    }

    public class CalculateRequest
    {
        public string? CouponCode { get; set; }
    }

    public class CalculateResponse
    {
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CheckoutSessionDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public List<CheckoutSessionItemDto> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string? CouponCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ReceiverName { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public Guid? OrderId { get; set; }
    }

    public class CheckoutSessionItemDto
    {
        public Guid Id { get; set; }
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
