using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace OrderService.Application.Orders.Queries.GetCheckoutSession
{
    public class GetCheckoutSessionHandler : IRequestHandler<GetCheckoutSessionQuery, CheckoutSessionDto>
    {
        private readonly ICheckoutSessionRepository _sessionRepository;
        private readonly ICurrentUserService _currentUser;

        public GetCheckoutSessionHandler(
            ICheckoutSessionRepository sessionRepository,
            ICurrentUserService currentUser)
        {
            _sessionRepository = sessionRepository;
            _currentUser = currentUser;
        }

        public async Task<CheckoutSessionDto> Handle(GetCheckoutSessionQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            var session = await _sessionRepository.GetByIdAsync(request.CheckoutId);
            if (session == null)
                throw new Exception("Checkout session not found");

            if (session.UserId != userId)
                throw new Exception("Forbidden: not your session");

            return new CheckoutSessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                Status = session.Status.ToString(),
                Items = session.Items.Select(i => new CheckoutSessionItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    SellerId = i.SellerId,
                    ProductName = i.ProductName,
                    Sku = i.Sku,
                    VariantName = i.VariantName,
                    ImageUrl = i.ImageUrl,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                SubTotal = session.SubTotal,
                ShippingFee = session.ShippingFee,
                Discount = session.Discount,
                TotalPrice = session.TotalPrice,
                CouponCode = session.CouponCode,
                Address = session.Address,
                Phone = session.Phone,
                ReceiverName = session.ReceiverName,
                PaymentMethod = session.PaymentMethod.ToString(),
                CreatedAt = session.CreatedAt,
                ExpiresAt = session.ExpiresAt,
                OrderId = session.OrderId
            };
        }
    }
}
