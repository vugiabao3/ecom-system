using System;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public class ShippingInfoDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public string? TrackingCode { get; set; }
        public string? ShipperId { get; set; }
        public string? ReceiverName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? FailureReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }

    public interface IShippingServiceClient
    {
        Task<ShippingInfoDto?> GetShippingByOrderId(Guid orderId);
    }
}
