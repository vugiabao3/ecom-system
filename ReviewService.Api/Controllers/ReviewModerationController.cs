using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api")]
public class ReviewModerationController : ControllerBase
{
    [HttpGet("seller/reviews/search")]
    public IActionResult SellerSearch() => Ok(new List<object>());
    [HttpGet("admin/reviews")]
    public IActionResult AdminList() => Ok(new List<object>());
    [HttpGet("admin/reviews/{id}")]
    public IActionResult AdminGet(Guid id) => Ok(new { id });
    [HttpGet("admin/reviews/pending")]
    public IActionResult Pending() => Ok(new List<object>());
    [HttpPost("admin/reviews/{id}/approve")]
    public IActionResult Approve(Guid id) => Ok(new { id, message = "Approved" });
}
