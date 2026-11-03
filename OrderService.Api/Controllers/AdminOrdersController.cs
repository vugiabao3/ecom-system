using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands.UpdateOrderStatus;
using OrderService.Application.Interfaces;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/admin/orders")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;
        public AdminOrdersController(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(new { page, pageSize, items = new List<object>() });
        }

        [HttpGet("{orderId:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Detail(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order);
        }

        [HttpGet("search")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Search([FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 10) => Ok(new { searchTerm, page, pageSize, items = new List<object>() });

        [HttpGet("statistics")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Statistics() => Ok(new { total = 0 });

        [HttpGet("pending")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Pending() => Ok(new List<object>());

        [HttpGet("cancelled")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Cancelled() => Ok(new List<object>());

        [HttpGet("disputed")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Disputed() => Ok(new List<object>());

        [HttpPatch("{orderId:guid}/status")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AdjustStatus(Guid orderId, [FromBody] UpdateOrderStatusCommand command)
        {
            command.OrderId = orderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{orderId:guid}/cancel")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Cancel(Guid orderId)
        {
            return Ok(new { orderId, message = "Cancelled" });
        }

        [HttpGet("{orderId:guid}/audit-logs")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AuditLogs(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order.StatusHistory);
        }

        [HttpGet("export")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Export() => Ok(new { message = "Export not implemented" });

        [HttpGet("revenue-summary")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult RevenueSummary() => Ok(new { revenue = 0 });
    }
}
