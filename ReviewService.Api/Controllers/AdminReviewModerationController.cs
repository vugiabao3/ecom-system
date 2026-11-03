using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/admin/reviews")]
public class AdminReviewModerationController : ControllerBase
{
    [HttpPost("{id}/reject")]
    public IActionResult Reject(Guid id) => Ok(new { id, message = "Rejected" });
    [HttpPost("{id}/hide")]
    public IActionResult Hide(Guid id) => Ok(new { id, message = "Hidden" });
    [HttpPost("{id}/restore")]
    public IActionResult Restore(Guid id) => Ok(new { id, message = "Restored" });
    [HttpGet("flagged")]
    public IActionResult Flagged() => Ok(new List<object>());
    [HttpGet("statistics")]
    public IActionResult Statistics() => Ok(new { pending = 0, published = 0, rejected = 0, hidden = 0, flagged = 0 });
}
