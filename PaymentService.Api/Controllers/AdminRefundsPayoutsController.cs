using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/admin/refunds")]
    public class AdminRefundsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult List() => Ok(new List<object>());
    }

    [ApiController]
    [Route("api/admin/payouts")]
    public class AdminPayoutsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult List() => Ok(new List<object>());

        [HttpPost("{payoutId}/approve")]
        [Authorize]
        public IActionResult Approve(Guid payoutId) => Ok(new { payoutId, message = "Approved" });

        [HttpPost("{payoutId}/reject")]
        [Authorize]
        public IActionResult Reject(Guid payoutId) => Ok(new { payoutId, message = "Rejected" });
    }
}
