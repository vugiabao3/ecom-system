using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EcomSystem.Contracts.Enums;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Queries.GetShippingOrders
{
    public class GetShippingOrdersHandler : IRequestHandler<GetShippingOrdersQuery, DTOs.PaginatedResponse<DTOs.OrderListItemDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUser;

        public GetShippingOrdersHandler(IOrderRepository orderRepository, ICurrentUserService currentUser)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
        }

        public async Task<DTOs.PaginatedResponse<DTOs.OrderListItemDto>> Handle(GetShippingOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize < 1 || request.PageSize > 100 ? 10 : request.PageSize;

            var (items, totalCount) = await _orderRepository.GetByUserIdPagedAsync(userId, page, pageSize, OrderStatus.Shipping.ToString());

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return new DTOs.PaginatedResponse<DTOs.OrderListItemDto>
            {
                Items = items.Select(o => new DTOs.OrderListItemDto
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
                }).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }
    }
}
