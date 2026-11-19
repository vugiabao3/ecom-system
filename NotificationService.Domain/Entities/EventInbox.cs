namespace NotificationService.Domain.Entities;
public class EventInbox
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EventId { get; set; } = default!;
    public string SourceService { get; set; } = default!;
    public string EventType { get; set; } = default!;
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}
