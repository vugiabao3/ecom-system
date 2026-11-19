namespace NotificationService.Domain.Entities;
public class NotificationDevice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = default!;
    public string DeviceToken { get; set; } = default!;
    public string? Platform { get; set; }
    public DateTime? LastActiveAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class QuietHours
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = default!;
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
    public string Timezone { get; set; } = "UTC";
}
