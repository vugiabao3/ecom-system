using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace OrderService.Application.Orders.Commands.ConfirmCheckout
{
    public class ConfirmCheckoutHandler : IRequestHandler<ConfirmCheckoutCommand, DTOs.OrderConfirmationDto?>
    {
        private readonly ICheckoutSessionRepository _sessionRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IInventoryServiceClient _inventoryClient;

        public ConfirmCheckoutHandler(
            ICheckoutSessionRepository sessionRepository,
            ICurrentUserService currentUser,
            IInventoryServiceClient inventoryClient)
        {
            _sessionRepository = sessionRepository;
            _currentUser = currentUser;
            _inventoryClient = inventoryClient;
        }

        public async Task<DTOs.OrderConfirmationDto?> Handle(ConfirmCheckoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Unauthorized");

            var session = await _sessionRepository.GetByIdAsync(request.CheckoutId);
            if (session == null)
                return null;

            if (!string.Equals(session.UserId, userId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Forbidden: not your session");

            if (session.Status == CheckoutSessionStatus.Completed)
                throw new UnauthorizedAccessException("Checkout session already completed");

            if (session.Status == CheckoutSessionStatus.Expired)
                throw new UnauthorizedAccessException("Checkout session has expired");

            // Validate stock is still available for all items.
            foreach (var item in session.Items)
            {
                var inventory = await _inventoryClient.GetInventoryByProductId(item.ProductId);
                if (inventory.Available < item.Quantity)
                    throw new UnauthorizedAccessException($"Insufficient stock for product {item.ProductName}");
            }

            return new DTOs.OrderConfirmationDto
            {
                CheckoutId = session.Id,
                OrderId = session.OrderId ?? Guid.Empty,
                Status = session.Status.ToString(),
                Message = "Checkout information confirmed. Stock is available. Proceed to payment."
            };
        }
    }
}
