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
    }
}
