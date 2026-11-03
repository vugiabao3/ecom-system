using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders.Queries.GetMyOrderDetail
{
    public class GetMyOrderDetailHandler : IRequestHandler<GetMyOrderDetailQuery, DTOs.OrderDetailDto?>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IShippingServiceClient _shippingClient;

        public GetMyOrderDetailHandler(IOrderRepository orderRepository, ICurrentUserService currentUser, IShippingServiceClient shippingClient)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
            _shippingClient = shippingClient;
        }

        public async Task<DTOs.OrderDetailDto?> Handle(GetMyOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Unauthorized");

            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                return null;

            if (!string.Equals(order.UserId, userId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Forbidden: not your order");

            var dto = new DTOs.OrderDetailDto
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
                }).OrderBy(x => x.CreatedAt).ToList() ?? new List<DTOs.OrderStatusHistoryDto>()
            };

            // Synchronize external shipping/return information through the ShippingService contract.
            // Best-effort: an unavailable ShippingService must not block order detail retrieval.
            dto.ShippingInfo = await _shippingClient.GetShippingByOrderId(order.Id);

            return dto;
        }
    }
}
