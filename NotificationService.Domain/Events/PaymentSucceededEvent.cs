namespace NotificationService.Domain.Events;

public class PaymentSucceededEvent
{
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
    public string? UserId { get; set; }
    public List<Application.DTOs.OrderItemDto>? Items { get; set; }
}