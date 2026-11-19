namespace NotificationService.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string? NavigationUrl { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
