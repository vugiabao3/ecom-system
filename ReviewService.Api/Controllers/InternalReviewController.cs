using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/internal")]
public class InternalReviewController : ControllerBase
{
    [HttpPost("reviews/eligibility/check")]
    public IActionResult EligibilityCheck() => Ok(new { eligible = true });
    [HttpGet("orders/{orderId}/review-eligibility")]
    public IActionResult OrderEligibility(Guid orderId) => Ok(new { orderId, eligibleItems = new object[0] });
    [HttpGet("products/{productId}/review-summary")]
    public IActionResult ProductSummary(Guid productId) => Ok(new { productId, average = 0, total = 0 });
    [HttpGet("shops/{sellerId}/review-summary")]
    public IActionResult ShopSummary(Guid sellerId) => Ok(new { sellerId, average = 0, total = 0 });
}
