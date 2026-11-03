using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/payment-methods")]
    public class PaymentMethodsController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get() => Ok(new[] { "COD", "QR" });

        [HttpGet("available")]
        [AllowAnonymous]
        public IActionResult Available() => Ok(new[] { "COD", "QR" });

        [HttpGet("{methodId}")]
        [AllowAnonymous]
        public IActionResult Detail(string methodId) => Ok(new { methodId });

        [HttpGet("configuration")]
        [AllowAnonymous]
        public IActionResult Configuration() => Ok(new { methods = new[] { "COD", "QR" } });
    }
}
