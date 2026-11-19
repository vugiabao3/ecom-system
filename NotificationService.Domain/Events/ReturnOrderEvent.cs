namespace NotificationService.Domain.Events;

public class ReturnOrderEvent
{
    public Guid OrderId { get; set; }
    public Guid ShipmentId { get; set; }
    public string? Reason { get; set; }
    public string? UserId { get; set; }
}