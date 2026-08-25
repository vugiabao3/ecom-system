using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcomSystem.Contracts.Enums;

namespace ShippingService.Domain.Entities
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid? ShipperId { get; set; }
        public string ReceiverName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string TrackingCode { get; set; }
        public ShipmentStatus Status { get; set; } = ShipmentStatus.Created;
        public string? FailureReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
