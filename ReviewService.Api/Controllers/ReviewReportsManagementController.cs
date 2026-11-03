using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api")]
public class ReviewReportsManagementController : ControllerBase
{
    [HttpGet("reports/me")]
    public IActionResult MyReports() => Ok(new List<object>());
    [HttpGet("reports/{id}")]
    public IActionResult Get(Guid id) => Ok(new { id });
    [HttpGet("admin/review-reports")]
    public IActionResult AdminList() => Ok(new List<object>());
    [HttpPost("admin/review-reports/{id}/resolve")]
    public IActionResult Resolve(Guid id) => Ok(new { id, message = "Resolved" });
    [HttpPost("admin/review-reports/{id}/dismiss")]
    public IActionResult Dismiss(Guid id) => Ok(new { id, message = "Dismissed" });
}
