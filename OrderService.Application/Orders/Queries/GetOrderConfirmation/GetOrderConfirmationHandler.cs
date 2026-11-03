using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders.Queries.GetOrderConfirmation
{
    public class GetOrderConfirmationHandler : IRequestHandler<GetOrderConfirmationQuery, DTOs.OrderDetailDto?>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUser;

        public GetOrderConfirmationHandler(IOrderRepository orderRepository, ICurrentUserService currentUser)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
        }

        public async Task<DTOs.OrderDetailDto?> Handle(GetOrderConfirmationQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Unauthorized");

            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                return null;

            if (!string.Equals(order.UserId, userId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Forbidden: not your order");

            return new DTOs.OrderDetailDto
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                PaymentStatus = order.PaymentStatus.ToString(),
                PaymentMethod = order.PaymentMethod.ToString(),
                SubTotal = order.SubTotal,
                ShippingFee = order.ShippingFee,
                Discount = order.Discount,
                TotalPrice = order.TotalPrice,
                Address = order.Address,
                Phone = order.Phone,
                ReceiverName = order.ReceiverName,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = order.Items?.Select(x => new DTOs.OrderDetailItemDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    SellerId = x.SellerId,
                    ProductName = x.ProductName,
                    Sku = x.Sku,
                    VariantName = x.VariantName,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price,
                    Quantity = x.Quantity
                }).ToList() ?? new List<DTOs.OrderDetailItemDto>(),
                StatusHistory = order.StatusHistory?.Select(x => new DTOs.OrderStatusHistoryDto
                {
                    Id = x.Id,
                    Status = x.Status,
                    Note = x.Note,
                    CreatedAt = x.CreatedAt
                }).OrderByDescending(x => x.CreatedAt).ToList() ?? new List<DTOs.OrderStatusHistoryDto>()
            };
        }
    }
}
