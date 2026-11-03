using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcomSystem.Contracts.Enums;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/internal/orders")]
    public class InternalOrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public InternalOrdersController(IOrderRepository orderRepository) { _orderRepository = orderRepository; }

        [HttpPost("{orderId:guid}/payment-status")]
        [Authorize]
        public async Task<IActionResult> PaymentStatus(Guid orderId, [FromBody] UpdatePaymentStatusRequest req)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            if (Enum.TryParse<PaymentStatus>(req.PaymentStatus, true, out var ps)) order.PaymentStatus = ps;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return Ok(new { message = "Updated" });
        }

        [HttpPost("{orderId:guid}/shipping-status")]
        [Authorize]
        public async Task<IActionResult> ShippingStatus(Guid orderId)
        {
            return Ok(new { orderId, message = "Synchronized" });
        }

        [HttpPost("{orderId:guid}/complete")]
        [Authorize]
        public async Task<IActionResult> Complete(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            order.Status = OrderStatus.Delivered;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return Ok(new { message = "Completed" });
        }

        [HttpPost("{orderId:guid}/cancel")]
        [Authorize]
        public async Task<IActionResult> Cancel(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            order.Status = OrderStatus.Cancelled;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return Ok(new { message = "Cancelled" });
        }

        [HttpGet("statistics")]
        [Authorize]
        public IActionResult Statistics() => Ok(new { total = 0 });

        [HttpGet("{orderId:guid}/summary")]
        [Authorize]
        public async Task<IActionResult> Summary(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(new { order.Id, order.Status, order.TotalPrice, order.UserId });
        }

        [HttpGet("{orderId:guid}")]
        [Authorize]
        public async Task<IActionResult> Detail(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order);
        }

        [HttpPost("batch")]
        [Authorize]
        public async Task<IActionResult> Batch([FromBody] List<Guid> ids)
        {
            var result = new List<object>();
            foreach (var id in ids.Take(100))
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order != null) result.Add(order);
            }
            return Ok(result);
        }

        [HttpGet("{orderId:guid}/exists")]
        [Authorize]
        public async Task<IActionResult> Exists(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            return Ok(new { orderId, exists = order != null });
        }

        [HttpGet("{orderId:guid}/items")]
        [Authorize]
        public async Task<IActionResult> Items(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order.Items);
        }
    }
}
