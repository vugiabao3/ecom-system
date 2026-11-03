using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders.Commands.UpdateOrderStatus;
using OrderService.Application.Orders.Queries.GetOrdersBySellerId;
using OrderService.Domain.Entities;
using EcomSystem.Contracts.Enums;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace OrderService.Api.Controllers
{
    public class RejectRequest { public string? Reason { get; set; } }
    public class NoteRequest { public string? Note { get; set; } }

    [ApiController]
    [Route("api/seller/orders")]
    public class SellerOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;
        private readonly IShippingServiceClient _shippingServiceClient;

        public SellerOrdersController(IMediator mediator, IOrderRepository orderRepository, IShippingServiceClient shippingServiceClient)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
            _shippingServiceClient = shippingServiceClient;
        }

        [HttpGet]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> GetSellerOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null || !Guid.TryParse(userId, out var sellerId)) return StatusCode(403, "Forbidden");
            var orders = await _mediator.Send(new GetOrdersBySellerIdQuery(sellerId));
            return Ok(orders);
        }

        [HttpGet("summary")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> GetSellerSummary()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null || !Guid.TryParse(userId, out var sellerId)) return StatusCode(403, "Forbidden");
            var orders = await _mediator.Send(new GetOrdersBySellerIdQuery(sellerId));
            return Ok(new { total = orders.Count, pending = orders.Count(o => o.Status.ToString() == OrderStatus.Pending.ToString()) });
        }

        [HttpGet("{orderId:guid}")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> GetSellerOrderDetail(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null || !Guid.TryParse(userId, out var sellerId)) return StatusCode(403, "Forbidden");
            if (!order.Items.Any(i => i.SellerId == sellerId)) return StatusCode(403, "Forbidden");
            return Ok(order);
        }

        [HttpGet("pending")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Pending() => await FilterByStatus(OrderStatus.Pending);

        [HttpGet("processing")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Processing() => await FilterByStatus(OrderStatus.Preparing);

        [HttpGet("shipping")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Shipping() => await FilterByStatus(OrderStatus.Shipping);

        [HttpGet("completed")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Completed() => await FilterByStatus(OrderStatus.Delivered);

        [HttpGet("cancelled")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Cancelled() => await FilterByStatus(OrderStatus.Cancelled);

        [HttpGet("search")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null || !Guid.TryParse(userId, out var sellerId)) return StatusCode(403, "Forbidden");
            var orders = await _mediator.Send(new GetOrdersBySellerIdQuery(sellerId));
            if (!string.IsNullOrWhiteSpace(searchTerm))
                orders = orders.Where(o => o.Id.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(orders);
        }

        [HttpPatch("{orderId:guid}/confirm")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Confirm(Guid orderId)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand { OrderId = orderId, Status = "Confirmed" });
            return Ok(result);
        }

        [HttpPatch("{orderId:guid}/reject")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Reject(Guid orderId, [FromBody] RejectRequest req)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand { OrderId = orderId, Status = "Cancelled", Note = req.Reason });
            return Ok(result);
        }

        [HttpPatch("{orderId:guid}/processing")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> ProcessingAction(Guid orderId)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand { OrderId = orderId, Status = "Preparing" });
            return Ok(result);
        }

        [HttpPatch("{orderId:guid}/ready-to-ship")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> ReadyToShip(Guid orderId)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand { OrderId = orderId, Status = "ReadyForShipment" });
            return Ok(result);
        }

        [HttpPatch("{orderId:guid}/ship")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Ship(Guid orderId)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand { OrderId = orderId, Status = "Shipping" });
            return Ok(result);
        }

        [HttpPatch("{orderId:guid}/complete")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Complete(Guid orderId)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand { OrderId = orderId, Status = "Delivered" });
            return Ok(result);
        }

        [HttpGet("{orderId:guid}/shipping-info")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> ShippingInfo(Guid orderId)
        {
            var info = await _shippingServiceClient.GetShippingByOrderId(orderId);
            if (info == null) return NotFound("Shipping info not available");
            return Ok(info);
        }

        [HttpGet("{orderId:guid}/history")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> History(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order.StatusHistory);
        }

        [HttpPost("{orderId:guid}/notes")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> AddNote(Guid orderId, [FromBody] NoteRequest req)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            order.StatusHistory.Add(new OrderStatusHistory { OrderId = order.Id, Status = order.Status.ToString(), Note = req.Note });
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return Ok(new { message = "Note added" });
        }
        private async Task<IActionResult> FilterByStatus(OrderStatus status)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null || !Guid.TryParse(userId, out var sellerId)) return StatusCode(403, "Forbidden");
            var orders = await _mediator.Send(new GetOrdersBySellerIdQuery(sellerId));
            var filtered = orders.Where(o => o.Status.ToString() == status.ToString()).ToList();
            return Ok(filtered);
        }
    }
}
