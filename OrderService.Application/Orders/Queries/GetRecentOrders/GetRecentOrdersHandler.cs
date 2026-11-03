using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Queries.GetRecentOrders
{
    public class GetRecentOrdersHandler : IRequestHandler<GetRecentOrdersQuery, DTOs.PaginatedResponse<DTOs.OrderListItemDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUser;

        public GetRecentOrdersHandler(IOrderRepository orderRepository, ICurrentUserService currentUser)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
        }

        public async Task<DTOs.PaginatedResponse<DTOs.OrderListItemDto>> Handle(GetRecentOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize < 1 || request.PageSize > 100 ? 10 : request.PageSize;

            var recentOrders = await _orderRepository.GetByUserIdRecentAsync(userId, request.Limit);

            var totalCount = recentOrders.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var pagedItems = recentOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new DTOs.OrderListItemDto
                {
                    Id = o.Id,
                    OrderNumber = o.Id.ToString().Substring(0, 8).ToUpper(),
                    Status = o.Status.ToString(),
                    PaymentStatus = o.PaymentStatus.ToString(),
                    PaymentMethod = o.PaymentMethod.ToString(),
                    TotalPrice = o.TotalPrice,
                    SubTotal = o.SubTotal,
                    ShippingFee = o.ShippingFee,
                    Discount = o.Discount,
                    CreatedAt = o.CreatedAt,
                    ReceiverName = o.ReceiverName,
                    Address = o.Address,
                    ItemCount = o.Items?.Count ?? 0,
                    FirstProductImage = o.Items?.FirstOrDefault()?.ImageUrl,
                    FirstProductName = o.Items?.FirstOrDefault()?.ProductName
                }).ToList();

            return new DTOs.PaginatedResponse<DTOs.OrderListItemDto>
            {
                Items = pagedItems,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }
    }
}
