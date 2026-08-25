using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Payments.Commands.ConfirmPayment;
using PaymentService.Application.Payments.Commands.FailPayment;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentActionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentActionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{id}/confirm")]
        [Authorize]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var result = await _mediator.Send(new ConfirmPaymentCommand { PaymentId = id });
            return Ok(new { confirmed = result });
        }

        [HttpPost("{id}/fail")]
        [Authorize]
        public async Task<IActionResult> Fail(Guid id, [FromBody] FailPaymentCommand command)
        {
            command.PaymentId = id;
            var result = await _mediator.Send(command);
            return Ok(new { failed = result });
        }
    }
}
