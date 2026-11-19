namespace NotificationService.Domain.Entities;
public class NotificationTemplate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TemplateKey { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string TitleTemplate { get; set; } = default!;
    public string MessageTemplate { get; set; } = default!;
    public string? Locale { get; set; } = "en";
    public string Channel { get; set; } = "InApp";
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
