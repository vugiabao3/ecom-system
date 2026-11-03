using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/seller/reviews")]
public class SellerReviewsDashboardController : ControllerBase
{
    [HttpGet]
    public IActionResult List() => Ok(new List<object>());
    [HttpGet("summary")]
    public IActionResult Summary() => Ok(new { average = 0, total = 0 });
    [HttpGet("statistics")]
    public IActionResult Statistics() => Ok(new { distribution = new object[0] });
    [HttpGet("low-rating")]
    public IActionResult LowRating() => Ok(new List<object>());
    [HttpGet("recent")]
    public IActionResult Recent() => Ok(new List<object>());
}
