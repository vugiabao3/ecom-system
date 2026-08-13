namespace ShippingService.Domain.Entities
{
    public class Shipment
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public string ReceiverName { get; set; }   // 👈 thêm
        public string Phone { get; set; }          // 👈 thêm
        public string Address { get; set; }

        public string TrackingCode { get; set; }   // 👈 rất quan trọng

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}