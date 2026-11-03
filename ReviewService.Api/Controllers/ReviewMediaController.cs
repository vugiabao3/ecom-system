using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/reviews/{reviewId}/media")]
public class ReviewMediaController : ControllerBase
{
    [HttpPost]
    public IActionResult Add(Guid reviewId) => Ok(new { reviewId, mediaId = Guid.NewGuid() });
    [HttpGet]
    public IActionResult List(Guid reviewId) => Ok(new List<object>());
}
