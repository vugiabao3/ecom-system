using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/reviews")]
public class BuyerReviewStatusController : ControllerBase
{
    [HttpGet("me/pending")]
    public IActionResult Pending() => Ok(new List<object>());
    [HttpGet("me/published")]
    public IActionResult Published() => Ok(new List<object>());
    [HttpGet("me/rejected")]
    public IActionResult Rejected() => Ok(new List<object>());
    [HttpGet("me/eligible-orders")]
    public IActionResult EligibleOrders() => Ok(new List<object>());
}
