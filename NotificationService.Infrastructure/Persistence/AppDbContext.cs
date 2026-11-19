using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
namespace NotificationService.Infrastructure.Persistence;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationPreference> NotificationPreferences { get; set; }
    public DbSet<NotificationDevice> NotificationDevices { get; set; }
    public DbSet<QuietHours> QuietHours { get; set; }
    public DbSet<EventInbox> EventInboxes { get; set; }
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
    public DbSet<NotificationCampaign> NotificationCampaigns { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notifications");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Type).IsRequired();
            entity.Property(x => x.Message).IsRequired();
            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.CreatedAt);
            entity.HasIndex(x => x.IsRead);
        });
        modelBuilder.Entity<NotificationPreference>().HasIndex(x => new { x.UserId, x.Category }).IsUnique();
        modelBuilder.Entity<NotificationDevice>().HasIndex(x => x.UserId);
        modelBuilder.Entity<EventInbox>().HasIndex(x => x.EventId).IsUnique();
        modelBuilder.Entity<NotificationTemplate>().HasIndex(x => x.TemplateKey).IsUnique();
    }
}
