using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.Interfaces;

namespace OrderService.Application.DTOs
{
    public class ConfirmCheckoutRequest
    {
        public Guid CheckoutId { get; set; }
    }

    public class OrderConfirmationDto
    {
        public Guid CheckoutId { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class OrderListItemDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ReceiverName { get; set; }
        public string Address { get; set; }
        public int ItemCount { get; set; }
        public string? FirstProductImage { get; set; }
        public string? FirstProductName { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }

    public class OrderSummaryDto
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ConfirmedOrders { get; set; }
        public int PreparingOrders { get; set; }
        public int ReadyForShipmentOrders { get; set; }
        public int ShippingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int ReturnedOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class OrderDetailItemDto
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
        public decimal Subtotal => Price * Quantity;
    }

    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ReceiverName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<OrderDetailItemDto> Items { get; set; } = new();
        public List<OrderStatusHistoryDto> StatusHistory { get; set; } = new();
        public ShippingInfoDto? ShippingInfo { get; set; }
    }

    public class OrderStatusHistoryDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
