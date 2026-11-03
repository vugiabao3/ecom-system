using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using EcomSystem.Contracts.Enums;

namespace OrderService.Application.Orders.Queries.GetOrderSummary
{
    public class GetOrderSummaryHandler : IRequestHandler<GetOrderSummaryQuery, DTOs.OrderSummaryDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUser;

        public GetOrderSummaryHandler(IOrderRepository orderRepository, ICurrentUserService currentUser)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
        }

        public async Task<DTOs.OrderSummaryDto> Handle(GetOrderSummaryQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            var orders = await _orderRepository.GetByUserIdAsync(userId);

            var summary = new DTOs.OrderSummaryDto
            {
                TotalOrders = orders.Count,
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                ConfirmedOrders = orders.Count(o => o.Status == OrderStatus.Confirmed),
                PreparingOrders = orders.Count(o => o.Status == OrderStatus.Preparing),
                ReadyForShipmentOrders = orders.Count(o => o.Status == OrderStatus.ReadyForShipment),
                ShippingOrders = orders.Count(o => o.Status == OrderStatus.Shipping),
                DeliveredOrders = orders.Count(o => o.Status == OrderStatus.Delivered),
                CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled),
                ReturnedOrders = orders.Count(o => o.Status == OrderStatus.Returned),
                TotalSpent = orders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.TotalPrice),
                AverageOrderValue = orders.Count > 0 ? orders.Sum(o => o.TotalPrice) / orders.Count : 0
            };

            return summary;
        }
    }
}
