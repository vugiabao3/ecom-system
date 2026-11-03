using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders.Queries.GetOrderTimeline
{
    public class GetOrderTimelineHandler : IRequestHandler<GetOrderTimelineQuery, List<DTOs.OrderStatusHistoryDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUser;

        public GetOrderTimelineHandler(IOrderRepository orderRepository, ICurrentUserService currentUser)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
        }

        public async Task<List<DTOs.OrderStatusHistoryDto>> Handle(GetOrderTimelineQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Unauthorized");

            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found");

            if (!string.Equals(order.UserId, userId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Forbidden: not your order");

            return order.StatusHistory?.Select(x => new DTOs.OrderStatusHistoryDto
            {
                Id = x.Id,
                Status = x.Status,
                Note = x.Note,
                CreatedAt = x.CreatedAt
            }).OrderBy(x => x.CreatedAt).ToList() ?? new List<DTOs.OrderStatusHistoryDto>();
        }
    }
}
