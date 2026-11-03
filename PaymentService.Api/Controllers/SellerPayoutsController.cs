using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/seller/balance")]
    public class SellerBalanceController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get() => Ok(new { balance = 0 });
    }

    [ApiController]
    [Route("api/seller/payouts")]
    public class SellerPayoutsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult List() => Ok(new List<object>());

        [HttpPost]
        [Authorize]
        public IActionResult Create() => Ok(new { message = "Payout requested" });

        [HttpGet("{payoutId}")]
        [Authorize]
        public IActionResult Detail(Guid payoutId) => Ok(new { payoutId });

        [HttpPost("{payoutId}/cancel")]
        [Authorize]
        public IActionResult Cancel(Guid payoutId) => Ok(new { payoutId, message = "Cancelled" });
    }
}
