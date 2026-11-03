using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/admin/payment-gateways")]
    public class AdminPaymentGatewaysController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult List() => Ok(new List<object>());

        [HttpGet("{provider}/health")]
        [Authorize]
        public IActionResult Health(string provider) => Ok(new { provider, healthy = true });
    }
}
