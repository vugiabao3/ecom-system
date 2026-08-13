namespace ShippingService.Application.Events
{
    public class ShippingCreatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid ShipmentId { get; set; }
    }
}
