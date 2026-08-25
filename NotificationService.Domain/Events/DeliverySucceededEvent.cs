namespace NotificationService.Domain.Events;

public class DeliverySucceededEvent
{
    public Guid OrderId { get; set; }
    public Guid ShipmentId { get; set; }
    public string PaymentMethod { get; set; }
}
