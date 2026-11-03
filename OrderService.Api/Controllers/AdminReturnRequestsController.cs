using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/admin/return-requests")]
    public class AdminReturnRequestsController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult List() => Ok(new List<object>());

        [HttpPost("{requestId:guid}/resolve")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Resolve(Guid requestId) => Ok(new { requestId, message = "Resolved" });
    }
}
