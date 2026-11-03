using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/reviews/{reviewId}/reports")]
public class ReviewReportsController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(Guid reviewId) => Ok(new { reviewId, reportId = Guid.NewGuid() });
}
