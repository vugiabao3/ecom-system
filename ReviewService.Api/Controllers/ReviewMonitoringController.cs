using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/internal")]
public class ReviewMonitoringController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "healthy" });
    [HttpGet("consumers/status")]
    public IActionResult Consumers() => Ok(new { connected = true });
    [HttpGet("events/failed")]
    public IActionResult FailedEvents() => Ok(new List<object>());
    [HttpPost("events/{eventId}/retry")]
    public IActionResult Retry(Guid eventId) => Ok(new { eventId, message = "Retried" });
    [HttpGet("statistics")]
    public IActionResult Statistics() => Ok(new { total = 0 });
}
