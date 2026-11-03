using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/seller/earnings")]
    public class SellerEarningsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get() => Ok(new { total = 0 });

        [HttpGet("summary")]
        [Authorize]
        public IActionResult Summary() => Ok(new { total = 0 });

        [HttpGet("transactions")]
        [Authorize]
        public IActionResult Transactions() => Ok(new List<object>());
    }
}
