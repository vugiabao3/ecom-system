using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Payments.Commands.ProcessPayment;


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

        // 🔥 FLOW 1
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProcessPayment(ProcessPaymentCommand request)
        {
            //// 🔥 LẤY TOKEN TỪ HEADER
            //var token = Request.Headers["Authorization"]
            //    .ToString()
            //    .Replace("Bearer ", "");

            // 🔥 NHÉT VÀO COMMAND
            var command = new ProcessPaymentCommand
            {
                OrderId = request.OrderId,
                //Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                //Items = request.Items,

                //Token = token // 👈 QUAN TRỌNG
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}