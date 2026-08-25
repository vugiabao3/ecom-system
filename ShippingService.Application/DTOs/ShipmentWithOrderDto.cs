using System;

namespace ShippingService.Application.DTOs
{
    public class ShipmentWithOrderDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public Guid? ShipperId { get; set; }
        public string ReceiverName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string TrackingCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? FailureReason { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
