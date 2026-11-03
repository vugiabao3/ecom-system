using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api")]
public class AdminReviewSearchController : ControllerBase
{
    [HttpGet("admin/reviews/search")]
    public IActionResult Search() => Ok(new List<object>());
}
