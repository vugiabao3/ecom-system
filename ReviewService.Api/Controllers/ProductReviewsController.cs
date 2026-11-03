using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/products/{productId}/reviews")]
public class ProductReviewsController : ControllerBase
{
    [HttpGet]
    public IActionResult List(Guid productId) => Ok(new List<object>());
    [HttpGet("summary")]
    public IActionResult Summary(Guid productId) => Ok(new { productId, average = 0, total = 0 });
    [HttpGet("distribution")]
    public IActionResult Distribution(Guid productId) => Ok(new { productId, stars = new object[0] });
    [HttpGet("recent")]
    public IActionResult Recent(Guid productId) => Ok(new List<object>());
    [HttpGet("top")]
    public IActionResult Top(Guid productId) => Ok(new List<object>());
    [HttpGet("with-media")]
    public IActionResult WithMedia(Guid productId) => Ok(new List<object>());
    [HttpGet("filter")]
    public IActionResult Filter(Guid productId) => Ok(new List<object>());
}
