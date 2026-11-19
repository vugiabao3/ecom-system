namespace ReviewService.Domain.Entities;
public enum ReviewStatus { Pending, Published, Rejected, Hidden, Flagged }
public class ProductReview
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderItemId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? VariantId { get; set; }
    public Guid BuyerId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Content { get; set; } = string.Empty;
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    public bool IsVerifiedPurchase { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PublishedAt { get; set; }
}
public class ShopReview
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SellerId { get; set; }
    public Guid BuyerId { get; set; }
    public Guid? OrderId { get; set; }
    public int OverallRating { get; set; }
    public int? PackagingRating { get; set; }
    public int? ServiceRating { get; set; }
    public int? SpeedRating { get; set; }
    public int? DescriptionAccuracyRating { get; set; }
    public string? Content { get; set; }
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
public class ReviewMedia
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReviewId { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int DisplayOrder { get; set; }
    public string UploadStatus { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class ReviewHelpfulVote
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReviewId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class SellerReviewResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReviewId { get; set; }
    public Guid SellerId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
public class ReviewReport
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReviewId { get; set; }
    public Guid ReporterId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class ReviewModerationHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReviewId { get; set; }
    public string Action { get; set; } = string.Empty;
    public Guid ModeratorId { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class ReviewEligibility
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderItemId { get; set; }
    public Guid BuyerId { get; set; }
    public bool IsReviewed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class ProductRatingSummary
{
    public Guid ProductId { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public int Star1 { get; set; }
    public int Star2 { get; set; }
    public int Star3 { get; set; }
    public int Star4 { get; set; }
    public int Star5 { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
public class ShopRatingSummary
{
    public Guid SellerId { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public int Star1 { get; set; }
    public int Star2 { get; set; }
    public int Star3 { get; set; }
    public int Star4 { get; set; }
    public int Star5 { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
public class ReviewEventInbox
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EventId { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string Status { get; set; } = "Received";
    public string? Error { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
