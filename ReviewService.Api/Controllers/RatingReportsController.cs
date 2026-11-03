using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api")]
public class RatingReportsController : ControllerBase
{
    [HttpGet("ratings/shops/{sellerId}")]
    public IActionResult ShopRating(Guid sellerId) => Ok(new { sellerId, average = 0, total = 0 });
    [HttpGet("ratings/products/top-rated")]
    public IActionResult TopRated() => Ok(new List<object>());
    [HttpGet("ratings/products/most-reviewed")]
    public IActionResult MostReviewed() => Ok(new List<object>());
    [HttpGet("reports/reviews/overview")]
    public IActionResult Overview() => Ok(new { total = 0 });
    [HttpGet("reports/reviews/trends")]
    public IActionResult Trends() => Ok(new List<object>());
}
