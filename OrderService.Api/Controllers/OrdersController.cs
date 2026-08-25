using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands.CancelOrder;
using OrderService.Application.Orders.Commands.CreateOrder;
using OrderService.Application.Orders.Commands.UpdateOrderStatus;
using OrderService.Application.Orders.EventHandlers;
using OrderService.Application.Orders.Queries.GetOrderById;
using OrderService.Application.Orders.Queries.GetOrdersByUserId;
using OrderService.Application.Orders.Queries.GetOrdersBySellerId;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using EcomSystem.Contracts.Enums;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IMediator mediator, IOrderRepository orderRepository)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
        }

        [HttpPost("checkout")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Checkout([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "User,Internal")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));
            return Ok(order);
        }

        [HttpPost("{id}/cancel")]
        [Authorize(AuthenticationSchemes = "User,Internal")]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelOrderCommand command)
        {
            command.OrderId = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        [Authorize(AuthenticationSchemes = "User,Internal")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusCommand command)
        {
            command.OrderId = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}/payment-status")]
        [Authorize(AuthenticationSchemes = "User,Internal")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid id, [FromBody] OrderService.Application.DTOs.UpdatePaymentStatusRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound("Order not found");

            order.PaymentStatus = Enum.Parse<PaymentStatus>(request.PaymentStatus, true);
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

            return Ok(new { message = "Payment status updated" });
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                                ?? User.FindFirst("sub")?.Value;
            if (currentUserId == null || currentUserId != userId)
                return StatusCode(403, "Forbidden");

            var result = await _mediator.Send(new GetOrdersByUserIdQuery(userId));
            return Ok(result);
        }

        [HttpGet("seller/{sellerId}")]
        [Authorize]
        public async Task<IActionResult> GetBySellerId(string sellerId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                         ?? User.FindFirst("sub")?.Value;
            if (userId == null || !Guid.TryParse(sellerId, out var sellerGuid) || sellerGuid.ToString() != userId)
                return StatusCode(403, "Forbidden");

            var result = await _mediator.Send(new GetOrdersBySellerIdQuery(Guid.Parse(sellerId)));
            return Ok(result);
        }
    }
}
