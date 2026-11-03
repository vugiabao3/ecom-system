using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/internal/payments")]
    public class InternalPaymentsController : ControllerBase
    {
        [HttpGet("{paymentId}/summary")]
        [Authorize]
        public IActionResult Summary(Guid paymentId) => Ok(new { paymentId, status = "Pending" });

        [HttpGet("order/{orderId}")]
        [Authorize]
        public IActionResult GetByOrder(Guid orderId) => Ok(new { orderId, paymentId = Guid.NewGuid() });

        [HttpPost("order/{orderId}/initialize")]
        [Authorize]
        public IActionResult Initialize(Guid orderId) => Ok(new { orderId, paymentId = Guid.NewGuid(), status = "Pending" });

        [HttpPost("{paymentId}/confirm")]
        [Authorize]
        public IActionResult Confirm(Guid paymentId) => Ok(new { paymentId, status = "Completed" });

        [HttpPost("{paymentId}/fail")]
        [Authorize]
        public IActionResult Fail(Guid paymentId) => Ok(new { paymentId, status = "Failed" });

        [HttpPost("{paymentId}/refund")]
        [Authorize]
        public IActionResult Refund(Guid paymentId) => Ok(new { paymentId, status = "RefundRequested" });

        [HttpGet("order/{orderId}/status")]
        [Authorize]
        public IActionResult Status(Guid orderId) => Ok(new { orderId, status = "Pending" });

        [HttpPost("cod/{paymentId}/confirm")]
        [Authorize]
        public IActionResult ConfirmCod(Guid paymentId) => Ok(new { paymentId, status = "Completed" });

        [HttpGet("order/{orderId}/transactions")]
        [Authorize]
        public IActionResult Transactions(Guid orderId) => Ok(new List<object>());

        [HttpGet("statistics")]
        [Authorize]
        public IActionResult Statistics() => Ok(new { total = 0 });
    }
}
