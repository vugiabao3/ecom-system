using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api")]
public class HelpfulVotesController : ControllerBase
{
    [HttpDelete("reviews/{reviewId}/helpful")]
    public IActionResult RemoveVote(Guid reviewId) => Ok(new { reviewId, message = "Removed" });
    [HttpGet("reviews/{reviewId}/helpful-count")]
    public IActionResult Count(Guid reviewId) => Ok(new { reviewId, count = 0 });
    [HttpGet("reviews/{reviewId}/my-vote")]
    public IActionResult MyVote(Guid reviewId) => Ok(new { reviewId, voted = false });
    [HttpGet("reviews/helpful/top")]
    public IActionResult Top() => Ok(new List<object>());
}
