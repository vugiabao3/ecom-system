using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/seller/return-requests")]
    public class SellerReturnRequestsController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "SellerOnly")]
        public IActionResult List() => Ok(new List<object>());

        [HttpPost("{requestId:guid}/approve")]
        [Authorize(Policy = "SellerOnly")]
        public IActionResult Approve(Guid requestId) => Ok(new { requestId, message = "Approved" });

        [HttpPost("{requestId:guid}/received")]
        [Authorize(Policy = "SellerOnly")]
        public IActionResult Received(Guid requestId) => Ok(new { requestId, message = "Received" });
        [Authorize(Policy = "SellerOnly")]
        public IActionResult Reject(Guid requestId, [FromBody] RejectRequest req) => Ok(new { requestId, message = "Rejected", reason = req.Reason });
    }
}
