using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Payments.Commands.ProcessPayment;
using PaymentService.Application.Payments.Commands.ConfirmPayment;
using PaymentService.Application.Payments.Commands.FailPayment;
using PaymentService.Application.Payments.Commands.ConfirmCashReceived;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("{id}/confirm")]
        [Authorize]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var result = await _mediator.Send(new ConfirmPaymentCommand { PaymentId = id });
            return Ok(result);
        }

        [HttpPost("{id}/fail")]
        [Authorize]
        public async Task<IActionResult> Fail(Guid id, [FromBody] FailPaymentCommand command)
        {
            command.PaymentId = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/confirm-cod")]
        [Authorize(Roles = "Shipper")]
        public async Task<IActionResult> ConfirmCod(Guid id)
        {
            var result = await _mediator.Send(new ConfirmCashReceivedCommand { OrderId = id });
            return Ok(new { confirmed = result });
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new PaymentService.Application.Payments.Queries.GetPayment.GetPaymentQuery { PaymentId = id });
            if (result == null) return NotFound("Payment not found");
            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Me() => Ok(new List<object>());

        [HttpGet("me/summary")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> MeSummary() => Ok(new { total = 0 });

        [HttpGet("me/{paymentId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> MeDetail(Guid paymentId)
        {
            var result = await _mediator.Send(new PaymentService.Application.Payments.Queries.GetPayment.GetPaymentQuery { PaymentId = paymentId });
            if (result == null) return NotFound("Payment not found");
            return Ok(result);
        }

        [HttpGet("me/search")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult MeSearch([FromQuery] string searchTerm) => Ok(new List<object>());

        [HttpGet("me/filter")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult MeFilter([FromQuery] string status) => Ok(new List<object>());
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Status(Guid id)
        {
            var result = await _mediator.Send(new PaymentService.Application.Payments.Queries.GetPayment.GetPaymentQuery { PaymentId = id });
            if (result == null) return NotFound("Payment not found");
            return Ok(new { id, status = result.Status.ToString() });
        }

        [HttpGet("{id}/result")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Result(Guid id)
        {
            var result = await _mediator.Send(new PaymentService.Application.Payments.Queries.GetPayment.GetPaymentQuery { PaymentId = id });
            if (result == null) return NotFound("Payment not found");
            return Ok(result);
        }

        [HttpPost("{id}/retry")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Retry(Guid id) => Ok(new { id, message = "Retry recorded" });

        [HttpPost("{id}/cancel")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Cancel(Guid id) => Ok(new { id, message = "Cancelled" });

        [HttpGet("order/{orderId}/methods")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult OrderMethods(Guid orderId) => Ok(new[] { "COD", "QR" });

        [HttpPost("cod")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult InitCod() => Ok(new { message = "COD initialized" });

        [HttpGet("cod/{paymentId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetCod(Guid paymentId)
        {
            var result = await _mediator.Send(new PaymentService.Application.Payments.Queries.GetPayment.GetPaymentQuery { PaymentId = paymentId });
            if (result == null) return NotFound("Payment not found");
            return Ok(result);
        }

        [HttpGet("cod/pending")]
        [Authorize]
        public IActionResult CodPending() => Ok(new List<object>());

        [HttpPost("cod/{paymentId}/confirm-received")]
        [Authorize]
        public IActionResult ConfirmReceived(Guid paymentId) => Ok(new { paymentId, message = "Received" });

        [HttpPost("cod/{paymentId}/report-failure")]
        [Authorize]
        public IActionResult ReportFailure(Guid paymentId) => Ok(new { paymentId, message = "Failure reported" });

        [HttpGet("cod/{paymentId}/collection-history")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult CodCollectionHistory(Guid paymentId) => Ok(new List<object>());

        [HttpPost("bank-transfer")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult InitBankTransfer() => Ok(new { message = "Bank transfer initialized" });

        [HttpGet("bank-transfer/{paymentId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetBankTransfer(Guid paymentId)
        {
            var result = await _mediator.Send(new PaymentService.Application.Payments.Queries.GetPayment.GetPaymentQuery { PaymentId = paymentId });
            if (result == null) return NotFound("Payment not found");
            return Ok(result);
        }

        [HttpGet("bank-transfer/{paymentId}/instructions")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult BankTransferInstructions(Guid paymentId) => Ok(new { paymentId, instructions = "Transfer to configured account" });

        [HttpGet("bank-transfer/{paymentId}/status")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> BankTransferStatus(Guid paymentId)
        {
            var result = await _mediator.Send(new PaymentService.Application.Payments.Queries.GetPayment.GetPaymentQuery { PaymentId = paymentId });
            if (result == null) return NotFound("Payment not found");
            return Ok(new { paymentId, status = result.Status.ToString() });
        }

        [HttpPost("bank-transfer/{paymentId}/verify")]
        [Authorize]
        public IActionResult VerifyBankTransfer(Guid paymentId) => Ok(new { paymentId, message = "Verification requested" });

        [HttpGet("bank-transfer/pending")]
        [Authorize]
        public IActionResult BankTransferPending() => Ok(new List<object>());

        [HttpPost("gateway/{provider}/create")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult GatewayCreate(string provider) => Ok(new { provider, message = "Created" });

        [HttpGet("gateway/{provider}/return")]
        [AllowAnonymous]
        public IActionResult GatewayReturn(string provider) => Ok(new { provider, message = "Return handled" });

        [HttpPost("gateway/{provider}/callback")]
        [AllowAnonymous]
        public IActionResult GatewayCallback(string provider) => Ok(new { provider, message = "Callback received" });

        [HttpPost("gateway/{provider}/webhook")]
        [AllowAnonymous]
        public IActionResult GatewayWebhook(string provider) => Ok(new { provider, message = "Webhook received" });

        [HttpGet("gateway/{provider}/status/{transactionId}")]
        [Authorize]
        public IActionResult GatewayStatus(string provider, string transactionId) => Ok(new { provider, transactionId, status = "Pending" });

        [HttpPost("gateway/{provider}/refund")]
        [Authorize]
        public IActionResult GatewayRefund(string provider) => Ok(new { provider, message = "Refund requested" });

        [HttpGet("gateway/providers")]
        [AllowAnonymous]
        public IActionResult GatewayProviders() => Ok(new[] { "COD", "QR" });

        [HttpGet("{paymentId}/refunds")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult PaymentRefunds(Guid paymentId) => Ok(new List<object>());

        [HttpGet("{paymentId}/transactions")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult PaymentTransactions(Guid paymentId) => Ok(new List<object>());

        [HttpPost("idempotency/check")]
        [Authorize]
        public IActionResult IdempotencyCheck() => Ok(new { exists = false });

        [HttpGet("{paymentId}/verification")]
        [Authorize]
        public IActionResult Verification(Guid paymentId) => Ok(new { paymentId, verified = false });

        [HttpPost("{paymentId}/verify")]
        [Authorize]
        public IActionResult Verify(Guid paymentId) => Ok(new { paymentId, message = "Verified" });

        [HttpGet("{paymentId}/attempts")]
        [Authorize]
        public IActionResult Attempts(Guid paymentId) => Ok(new List<object>());

        [HttpPost("{paymentId}/attempts")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult CreateAttempt(Guid paymentId) => Ok(new { paymentId, message = "Attempt created" });

        [HttpGet("security/events")]
        [Authorize]
        public IActionResult SecurityEvents() => Ok(new List<object>());

        [HttpGet("me/pending")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult Pending() => Ok(new List<object>());

        [HttpGet("me/successful")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult Successful() => Ok(new List<object>());

        [HttpGet("me/failed")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult Failed() => Ok(new List<object>());

        [HttpGet("me/refunded")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult Refunded() => Ok(new List<object>());

        [HttpGet("me/{paymentId}/history")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> History(Guid paymentId)
        {
            var result = await _mediator.Send(new PaymentService.Application.Payments.Queries.GetPayment.GetPaymentQuery { PaymentId = paymentId });
            if (result == null) return NotFound("Payment not found");
            return Ok(new List<object>());
        }

        [HttpGet("{id}/receipt")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Receipt(Guid id)
        {
            var result = await _mediator.Send(new PaymentService.Application.Payments.Queries.GetPayment.GetPaymentQuery { PaymentId = id });
            if (result == null) return NotFound("Payment not found");
            return Ok(result);
        }

    }
}
