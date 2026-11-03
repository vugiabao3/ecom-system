using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/reviews")]
public class BuyerReviewCenterController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult MyReviews() => Ok(new List<object>());
}
