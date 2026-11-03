using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands.Checkout;
using OrderService.Application.Orders.Commands.ConfirmCheckout;
using OrderService.Application.Orders.Commands.CancelOrder;
using OrderService.Application.Orders.Commands.UpdateOrderStatus;
using OrderService.Application.Orders.Queries.GetOrderById;
using OrderService.Application.Orders.Queries.GetOrdersByUserId;
using OrderService.Application.Orders.Queries.GetOrdersBySellerId;
using OrderService.Application.Orders.Queries.PreviewOrder;
using OrderService.Application.Orders.Queries.ValidateOrder;
using OrderService.Application.Orders.Queries.CalculateOrder;
using OrderService.Application.Orders.Queries.GetCheckoutSession;
using OrderService.Application.Orders.Queries.GetOrderConfirmation;
using OrderService.Application.Orders.Queries.GetMyOrders;
using OrderService.Application.Orders.Queries.GetOrderSummary;
using OrderService.Application.Orders.Queries.GetMyOrderDetail;
using OrderService.Application.Orders.Queries.GetRecentOrders;
using OrderService.Application.Orders.Queries.SearchOrders;
using OrderService.Application.Orders.Queries.FilterOrdersByStatus;
using OrderService.Application.Orders.Queries.GetPendingOrders;
using OrderService.Application.Orders.Queries.GetProcessingOrders;
using OrderService.Application.Orders.Queries.GetShippingOrders;
using OrderService.Application.Orders.Queries.GetCompletedOrders;
using OrderService.Application.Orders.Queries.GetCancelledOrders;
using OrderService.Application.Orders.Queries.GetReturnedOrders;
using OrderService.Application.Orders.Queries.GetOrderTimeline;
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
        private readonly IShippingServiceClient _shippingServiceClient;

        public OrdersController(IMediator mediator, IOrderRepository orderRepository, IShippingServiceClient shippingServiceClient)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
            _shippingServiceClient = shippingServiceClient;
        }

        // API 1: Create an order from selected cart items (idempotent)
        [HttpPost("checkout")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // API 2: Preview an order before placing it
        [HttpPost("preview")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Preview([FromBody] PreviewOrderQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // API 3: Validate order conditions
        [HttpPost("validate")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Validate([FromBody] ValidateOrderQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // API 4: Calculate order totals
        [HttpPost("calculate")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Calculate([FromBody] CalculateOrderQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // API 5: Retrieve checkout session information
        [HttpGet("checkout/{checkoutId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetCheckoutSession(Guid checkoutId)
        {
            var result = await _mediator.Send(new GetCheckoutSessionQuery { CheckoutId = checkoutId });
            return Ok(result);
        }

        // API 6: Confirm checkout information before payment
        [HttpPost("checkout/confirm")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> ConfirmCheckout([FromBody] ConfirmCheckoutCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (result == null)
                    return NotFound("Checkout session not found");
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        // API 7: Retrieve order confirmation after creation
        [HttpGet("{id}/confirmation")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetOrderConfirmation(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetOrderConfirmationQuery { OrderId = id });
                if (result == null)
                    return NotFound("Order not found");
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        // API 8: Get all orders belonging to the authenticated buyer (paginated)
        [HttpGet("me")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetMyOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? statusFilter = null)
        {
            var result = await _mediator.Send(new GetMyOrdersQuery { Page = page, PageSize = pageSize, StatusFilter = statusFilter });
            return Ok(result);
        }

        // API 9: Get buyer order statistics
        [HttpGet("me/summary")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetOrderSummary()
        {
            var result = await _mediator.Send(new GetOrderSummaryQuery());
            return Ok(result);
        }

        // API 10: Get buyer order details with ownership check
        [HttpGet("me/{orderId:guid}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetMyOrderDetail(Guid orderId)
        {
            try
            {
                var result = await _mediator.Send(new GetMyOrderDetailQuery { OrderId = orderId });
                if (result == null)
                    return NotFound("Order not found");
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        // API 11: Get recent orders
        [HttpGet("me/recent")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetRecentOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int limit = 10)
        {
            var result = await _mediator.Send(new GetRecentOrdersQuery { Page = page, PageSize = pageSize, Limit = limit });
            return Ok(result);
        }

        // API 12: Search buyer orders by order number
        [HttpGet("me/search")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> SearchOrders([FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new SearchOrdersQuery { SearchTerm = searchTerm, Page = page, PageSize = pageSize });
            return Ok(result);
        }

        // API 13: Filter buyer orders by status
        [HttpGet("me/filter")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> FilterOrdersByStatus([FromQuery] string statusFilter, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new FilterOrdersByStatusQuery { StatusFilter = statusFilter, Page = page, PageSize = pageSize });
            return Ok(result);
        }

        // API 14: Get pending-confirmation orders
        [HttpGet("me/pending")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetPendingOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetPendingOrdersQuery { Page = page, PageSize = pageSize });
            return Ok(result);
        }

        // API 15: Get processing orders
        [HttpGet("me/processing")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetProcessingOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetProcessingOrdersQuery { Page = page, PageSize = pageSize });
            return Ok(result);
        }

        // API 16: Get shipping orders
        [HttpGet("me/shipping")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetShippingOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetShippingOrdersQuery { Page = page, PageSize = pageSize });
            return Ok(result);
        }

        // API 17: Get completed (delivered) orders
        [HttpGet("me/completed")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetCompletedOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetCompletedOrdersQuery { Page = page, PageSize = pageSize });
            return Ok(result);
        }

        // API 18: Get cancelled orders
        [HttpGet("me/cancelled")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetCancelledOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetCancelledOrdersQuery { Page = page, PageSize = pageSize });
            return Ok(result);
        }

        // API 19: Get returned orders
        [HttpGet("me/returned")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetReturnedOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetReturnedOrdersQuery { Page = page, PageSize = pageSize });
            return Ok(result);
        }

        // API 20: Get order status timeline (history)
        [HttpGet("me/{orderId:guid}/invoice")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetInvoice(Guid orderId)
        {
            try
            {
                var result = await _mediator.Send(new GetMyOrderDetailQuery { OrderId = orderId });
                if (result == null) return NotFound("Order not found");
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, ex.Message); }
        }

        [HttpGet("me/{orderId:guid}/tracking")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetTracking(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null || !string.Equals(order.UserId, userId, StringComparison.OrdinalIgnoreCase))
                return StatusCode(403, "Forbidden");
            var shipping = await _shippingServiceClient.GetShippingByOrderId(orderId);
            if (shipping == null) return NotFound("Tracking not available");
            return Ok(shipping);
        }

        [HttpGet("{orderId:guid}/cancellation-reasons")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult GetCancellationReasons(Guid orderId)
        {
            var reasons = new[] { "Changed mind", "Found better price", "Ordered by mistake", "Delivery too slow", "Other" };
            return Ok(reasons);
        }

        [HttpGet("{orderId:guid}/cancellation")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetCancellation(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null || !string.Equals(order.UserId, userId, StringComparison.OrdinalIgnoreCase))
                return StatusCode(403, "Forbidden");
            var cancelled = order.StatusHistory?.Where(x => x.Status == "Cancelled").OrderBy(x => x.CreatedAt).LastOrDefault();
            return Ok(new { orderId, status = order.Status.ToString(), cancelledAt = cancelled?.CreatedAt, note = cancelled?.Note });
        }

        [HttpPost("{orderId:guid}/cancel-request")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> CancelRequest(Guid orderId)
        {
            return Ok(new { orderId, status = "Pending" });
        }

        [HttpPost("{orderId:guid}/cancel-request/withdraw")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> WithdrawCancelRequest(Guid orderId)
        {
            return Ok(new { orderId, status = "Withdrawn" });
        }

        [HttpGet("{orderId:guid}/status")]
        [Authorize]
        public async Task<IActionResult> GetStatus(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(new { orderId, status = order.Status.ToString() });
        }

        [HttpGet("{orderId:guid}/status-history")]
        [Authorize]
        public async Task<IActionResult> GetStatusHistory(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order.StatusHistory?.OrderBy(x => x.CreatedAt).ToList());
        }

        [HttpPost("{orderId:guid}/status/transition")]
        [Authorize]
        public async Task<IActionResult> TransitionStatus(Guid orderId, [FromBody] UpdateOrderStatusCommand command)
        {
            command.OrderId = orderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{orderId:guid}/payment")]
        [Authorize]
        public async Task<IActionResult> GetPayment(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(new { orderId, paymentStatus = order.PaymentStatus.ToString(), paymentMethod = order.PaymentMethod.ToString(), total = order.TotalPrice });
        }

        [HttpGet("{orderId:guid}/payment-status")]
        [Authorize]
        public async Task<IActionResult> GetPaymentStatus(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(new { orderId, paymentStatus = order.PaymentStatus.ToString() });
        }

        [HttpPost("{orderId:guid}/payment/failure")]
        [Authorize]
        public async Task<IActionResult> PaymentFailure(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            order.PaymentStatus = PaymentStatus.Failed;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return Ok(new { message = "Payment failure recorded" });
        }

        [HttpPost("{orderId:guid}/payment/refund-request")]
        [Authorize]
        public async Task<IActionResult> RefundRequest(Guid orderId)
        {
            return Ok(new { orderId, message = "Refund request recorded" });
        }

        [HttpGet("{orderId:guid}/payment/history")]
        [Authorize]
        public async Task<IActionResult> PaymentHistory(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order.StatusHistory);
        }

        [HttpGet("{orderId:guid}/shipping")]
        [Authorize]
        public async Task<IActionResult> GetShipping(Guid orderId)
        {
            var info = await _shippingServiceClient.GetShippingByOrderId(orderId);
            if (info == null) return NotFound("Shipping not found");
            return Ok(info);
        }

        [HttpGet("{orderId:guid}/shipping/tracking")]
        [Authorize]
        public async Task<IActionResult> GetShippingTracking(Guid orderId)
        {
            var info = await _shippingServiceClient.GetShippingByOrderId(orderId);
            if (info == null) return NotFound("Tracking not found");
            return Ok(info);
        }

        [HttpPost("{orderId:guid}/shipping/confirm")]
        [Authorize]
        public async Task<IActionResult> ShippingConfirm(Guid orderId)
        {
            return Ok(new { orderId, message = "Shipping confirmed" });
        }

        [HttpPost("{orderId:guid}/shipping/delivered")]
        [Authorize]
        public async Task<IActionResult> ShippingDelivered(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            order.Status = OrderStatus.Delivered;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return Ok(new { message = "Delivery recorded" });
        }

        [HttpPost("{orderId:guid}/shipping/failed")]
        [Authorize]
        public async Task<IActionResult> ShippingFailed(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            order.Status = OrderStatus.DeliveryFailed;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return Ok(new { message = "Delivery failure recorded" });
        }

        [HttpPost("{orderId:guid}/shipping/returned")]
        [Authorize]
        public async Task<IActionResult> ShippingReturned(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            order.Status = OrderStatus.Returned;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return Ok(new { message = "Return recorded" });
        }

        [HttpGet("{orderId:guid}/shipping/history")]
        [Authorize]
        public async Task<IActionResult> ShippingHistory(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order.StatusHistory);
        }

        [HttpPost("{orderId:guid}/return-requests")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> CreateReturnRequest(Guid orderId)
        {
            return Ok(new { orderId, message = "Return request created" });
        }

        [HttpGet("{orderId:guid}/return-requests")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> ListReturnRequests(Guid orderId)
        {
            return Ok(new List<object>());
        }

        [HttpGet("/api/return-requests/{requestId:guid}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetReturnRequest(Guid requestId)
        {
            return Ok(new { requestId, message = "Return request details" });
        }

        [HttpPost("/api/return-requests/{requestId:guid}/evidence")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UploadEvidence(Guid requestId, IFormFile file)
        {
            return Ok(new { requestId, fileName = file.FileName, message = "Evidence uploaded" });
        }

        [HttpPost("/api/return-requests/{requestId:guid}/withdraw")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> WithdrawReturnRequest(Guid requestId)
        {
            return Ok(new { requestId, message = "Withdrawn" });
        }

        [HttpGet("{orderId:guid}/refunds")]
        [Authorize]
        public async Task<IActionResult> GetRefunds(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order.StatusHistory);
        }

        [HttpPost("{orderId:guid}/refunds")]
        [Authorize]
        public async Task<IActionResult> InitiateRefund(Guid orderId)
        {
            return Ok(new { orderId, message = "Refund request initiated" });
        }

        [HttpGet("{orderId:guid}/review-eligibility")]
        [Authorize]
        public async Task<IActionResult> ReviewEligibility(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            var eligible = order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Returned;
            return Ok(new { orderId, eligible });
        }

        [HttpGet("{orderId:guid}/reviewed-items")]
        [Authorize]
        public async Task<IActionResult> ReviewedItems(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");
            return Ok(order.Items);
        }

        [HttpGet("me/review-pending")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> ReviewPending()
        {
            return Ok(new List<object>());
        }

        [HttpGet("me/{orderId:guid}/timeline")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetOrderTimeline(Guid orderId)
        {
            try
            {
                var result = await _mediator.Send(new GetOrderTimelineQuery { OrderId = orderId });
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
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
