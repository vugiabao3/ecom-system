using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Entities;
namespace ReviewService.Infrastructure.Persistence;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ShopReview> ShopReviews { get; set; }
    public DbSet<ReviewMedia> ReviewMedia { get; set; }
    public DbSet<ReviewHelpfulVote> ReviewHelpfulVotes { get; set; }
    public DbSet<SellerReviewResponse> SellerReviewResponses { get; set; }
    public DbSet<ReviewReport> ReviewReports { get; set; }
    public DbSet<ReviewModerationHistory> ReviewModerationHistory { get; set; }
    public DbSet<ReviewEligibility> ReviewEligibilities { get; set; }
    public DbSet<ProductRatingSummary> ProductRatingSummaries { get; set; }
    public DbSet<ShopRatingSummary> ShopRatingSummaries { get; set; }
    public DbSet<ReviewEventInbox> ReviewEventInbox { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewHelpfulVote>().HasIndex(x => new { x.ReviewId, x.UserId }).IsUnique();
        modelBuilder.Entity<ProductRatingSummary>().HasKey(x => x.ProductId);
        modelBuilder.Entity<ShopRatingSummary>().HasKey(x => x.SellerId);
    }
}
