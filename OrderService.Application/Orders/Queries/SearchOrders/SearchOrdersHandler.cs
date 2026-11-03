using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Queries.SearchOrders
{
    public class SearchOrdersHandler : IRequestHandler<SearchOrdersQuery, DTOs.PaginatedResponse<DTOs.OrderListItemDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUser;

        public SearchOrdersHandler(IOrderRepository orderRepository, ICurrentUserService currentUser)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
        }

        public async Task<DTOs.PaginatedResponse<DTOs.OrderListItemDto>> Handle(SearchOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize < 1 || request.PageSize > 100 ? 10 : request.PageSize;

            var searchTerm = string.IsNullOrWhiteSpace(request.SearchTerm) ? string.Empty : request.SearchTerm.Trim();

            var (items, totalCount) = await _orderRepository.SearchByUserIdAsync(userId, searchTerm, page, pageSize);

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
