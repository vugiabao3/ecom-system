namespace NotificationService.Domain.Events;

public class ShippingCreatedEvent
{
    public Guid OrderId { get; set; }
    public Guid ShipmentId { get; set; }
    public string? UserId { get; set; }
}