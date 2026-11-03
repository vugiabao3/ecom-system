using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api")]
public class RatingAggregationController : ControllerBase
{
    [HttpPost("internal/reviews/aggregate/recalculate")]
    public IActionResult Recalculate() => Ok(new { message = "Recalculated" });
    [HttpPost("internal/reviews/events")]
    public IActionResult ReceiveEvent() => Ok(new { message = "Received" });
    [HttpGet("internal/reviews/{reviewId}/summary")]
    public IActionResult ReviewSummary(Guid reviewId) => Ok(new { reviewId });
    [HttpGet("internal/reviews/statistics")]
    public IActionResult InternalStatistics() => Ok(new { total = 0 });
    [HttpGet("ratings/products/{productId}")]
    public IActionResult ProductRating(Guid productId) => Ok(new { productId, average = 0, total = 0 });
}
