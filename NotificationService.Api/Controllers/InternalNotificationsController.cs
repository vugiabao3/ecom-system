using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Api.Controllers
{
    [ApiController]
    [Route("api/internal")]
    public class InternalNotificationsController : ControllerBase
    {
        [HttpPost("notifications")]
        [Authorize]
        public IActionResult Create() => Ok(new { message = "Created" });

        [HttpPost("notifications/batch")]
        [Authorize]
        public IActionResult Batch() => Ok(new { message = "Batch created" });

        [HttpPost("notifications/broadcast")]
        [Authorize]
        public IActionResult Broadcast() => Ok(new { message = "Broadcast queued" });

        [HttpPost("events")]
        [Authorize]
        public IActionResult Event() => Ok(new { message = "Event accepted" });

        [HttpPost("events/batch")]
        [Authorize]
        public IActionResult EventsBatch() => Ok(new { message = "Events accepted" });

        [HttpGet("notifications/{id}")]
        [Authorize]
        public IActionResult GetNotification(Guid id) => Ok(new { id });

        [HttpGet("notifications/users/{userId}/summary")]
        [Authorize]
        public IActionResult UserSummary(Guid userId) => Ok(new { userId, total = 0, unread = 0 });

        [HttpGet("notifications/statistics")]
        [Authorize]
        public IActionResult Statistics() => Ok(new { total = 0, sent = 0, failed = 0 });

        [HttpGet("queue/statistics")]
        [Authorize]
        public IActionResult QueueStatistics() => Ok(new { depth = 0, consumers = 0 });

        [HttpGet("queue/failed")]
        [Authorize]
        public IActionResult QueueFailed() => Ok(new List<object>());

        [HttpPost("queue/{messageId}/retry")]
        [Authorize]
        public IActionResult QueueRetry(Guid messageId) => Ok(new { messageId, message = "Retry queued" });

        [HttpGet("consumers/status")]
        [Authorize]
        public IActionResult ConsumersStatus() => Ok(new { status = "Connected" });

        [HttpGet("statistics")]
        [Authorize]
        public IActionResult InternalStatistics() => Ok(new { total = 0 });
    }
}
