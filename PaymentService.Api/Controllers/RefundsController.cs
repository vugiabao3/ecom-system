using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/refunds")]
    public class RefundsController : ControllerBase
    {
        [HttpGet("{refundId}")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult Get(Guid refundId) => Ok(new { refundId });

        [HttpGet("{refundId}/status")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult Status(Guid refundId) => Ok(new { refundId, status = "Pending" });

        [HttpGet("me")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult Me() => Ok(new List<object>());

        [HttpPost("{refundId}/approve")]
        [Authorize]
        public IActionResult Approve(Guid refundId) => Ok(new { refundId, message = "Approved" });

        [HttpPost("{refundId}/reject")]
        [Authorize]
        public IActionResult Reject(Guid refundId) => Ok(new { refundId, message = "Rejected" });

        [HttpPost("{refundId}/cancel")]
        [Authorize]
        public IActionResult Cancel(Guid refundId) => Ok(new { refundId, message = "Cancelled" });

        [HttpGet("{refundId}/history")]
        [Authorize]
        public IActionResult History(Guid refundId) => Ok(new List<object>());
        [Authorize]
        public IActionResult Process(Guid refundId) => Ok(new { refundId, message = "Processed" });
    }
}
