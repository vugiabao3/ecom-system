using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api")]
public class ShopReviewsController : ControllerBase
{
    [HttpPost("shop-reviews")]
    public IActionResult Create() => Ok(new { id = Guid.NewGuid() });
    [HttpGet("shop-reviews/{id}")]
    public IActionResult Get(Guid id) => Ok(new { id });
    [HttpPut("shop-reviews/{id}")]
    public IActionResult Edit(Guid id) => Ok(new { id, message = "Updated" });
    [HttpDelete("shop-reviews/{id}")]
    public IActionResult Delete(Guid id) => Ok(new { id, message = "Deleted" });
    [HttpGet("shops/{sellerId}/reviews")]
    public IActionResult List(Guid sellerId) => Ok(new List<object>());
    [HttpGet("shops/{sellerId}/reviews/summary")]
    public IActionResult Summary(Guid sellerId) => Ok(new { sellerId, average = 0, total = 0 });
}
