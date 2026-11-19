namespace NotificationService.Domain.Entities;
public class NotificationPreference
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = default!;
    public string Category { get; set; } = default!;
    public bool InApp { get; set; } = true;
    public bool Email { get; set; } = false;
    public bool Push { get; set; } = false;
    public bool Sms { get; set; } = false;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
