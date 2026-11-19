namespace NotificationService.Domain.Events;

public class PaymentFailedEvent
{
    public Guid OrderId { get; set; }
    public string? UserId { get; set; }
}