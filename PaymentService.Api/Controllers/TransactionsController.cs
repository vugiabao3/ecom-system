using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        [HttpGet("search")]
        [Authorize]
        public IActionResult Search([FromQuery] string searchTerm) => Ok(new List<object>());
        [Authorize(Policy = "UserOnly")]
        public IActionResult Me() => Ok(new List<object>());

        [HttpGet("{transactionId}/status")]
        [Authorize]
        public IActionResult Status(Guid transactionId) => Ok(new { transactionId, status = "Pending" });

        [HttpGet("{transactionId}/history")]
        [Authorize]
        public IActionResult History(Guid transactionId) => Ok(new List<object>());

        [HttpPost("{transactionId}/reconcile")]
        [Authorize]
        public IActionResult Reconcile(Guid transactionId) => Ok(new { transactionId, message = "Reconciled" });
    }
}
