namespace NotificationService.Domain.Entities;
public class NotificationCampaign
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? TemplateId { get; set; }
    public string Audience { get; set; } = default!;
    public string Category { get; set; } = default!;
    public DateTime? ScheduleTime { get; set; }
    public string Status { get; set; } = "Draft";
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
