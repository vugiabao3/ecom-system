namespace NotificationService.Domain.Events;

public class DeliveryFailedEvent
{
    public Guid OrderId { get; set; }
    public Guid ShipmentId { get; set; }
    public string? Reason { get; set; }
}
