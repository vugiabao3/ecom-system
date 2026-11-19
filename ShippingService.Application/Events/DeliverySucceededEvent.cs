namespace ShippingService.Application.Events
{
    public class DeliverySucceededEvent
    {
        public Guid OrderId { get; set; }
        public Guid ShipmentId { get; set; }
        public string PaymentMethod { get; set; }
        public string? UserId { get; set; }
    }

    public class DeliveryFailedEvent
    {
        public Guid OrderId { get; set; }
        public Guid ShipmentId { get; set; }
        public string? Reason { get; set; }
        public string? UserId { get; set; }
    }
}