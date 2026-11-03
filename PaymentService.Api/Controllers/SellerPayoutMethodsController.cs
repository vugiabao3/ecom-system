using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/seller/payout-methods")]
    public class SellerPayoutMethodsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult List() => Ok(new List<object>());

        [HttpPost]
        [Authorize]
        public IActionResult Create() => Ok(new { message = "Created" });

        [HttpPut("{methodId}")]
        [Authorize]
        public IActionResult Update(Guid methodId) => Ok(new { methodId, message = "Updated" });

        [HttpDelete("{methodId}")]
        [Authorize]
        public IActionResult Delete(Guid methodId) => Ok(new { methodId, message = "Deleted" });
    }

    [ApiController]
    [Route("api/admin/payments")]
    public class AdminPaymentsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult List() => Ok(new List<object>());

        [HttpGet("{paymentId}")]
        [Authorize]
        public IActionResult Detail(Guid paymentId) => Ok(new { paymentId });

        [HttpGet("search")]
        [Authorize]
        public IActionResult Search() => Ok(new List<object>());

        [HttpGet("statistics")]
        [Authorize]
        public IActionResult Statistics() => Ok(new { total = 0 });

        [HttpGet("pending")]
        [Authorize]
        public IActionResult Pending() => Ok(new List<object>());

        [HttpGet("refunded")]
        [Authorize]
        public IActionResult Refunded() => Ok(new List<object>());

        [HttpGet("transactions")]
        [Authorize]
        public IActionResult Transactions() => Ok(new List<object>());

        [HttpGet("transactions/reconciliation")]
        [Authorize]
        public IActionResult Reconciliation() => Ok(new List<object>());

        [HttpGet("export")]
        [Authorize]
        public IActionResult Export() => Ok(new { format = "csv", message = "Exported" });
    }
}
