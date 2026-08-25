using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;

namespace CartService.Application.Cart.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityHandler : IRequestHandler<UpdateCartItemQuantityCommand, UpdateCartItemQuantityResponse>
    {
        private readonly ICartRepository _cartRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly IInventoryServiceClient _inventoryClient;

        public UpdateCartItemQuantityHandler(
            ICartRepository cartRepo,
            ICurrentUserService currentUser,
            IInventoryServiceClient inventoryClient)
        {
            _cartRepo = cartRepo;
            _currentUser = currentUser;
            _inventoryClient = inventoryClient;
        }

        public async Task<UpdateCartItemQuantityResponse> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            var cartItem = await _cartRepo.GetItemAsync(userId, request.ProductId);

            if (cartItem == null)
                throw new Exception("Item not found in cart");

            if (request.Quantity <= 0)
            {
                await _cartRepo.DeleteAsync(cartItem);
                await _cartRepo.SaveChangesAsync();

                return new UpdateCartItemQuantityResponse
                {
                    Message = "Item removed"
                };
            }

            var oldQuantity = cartItem.Quantity;
            var quantityDiff = request.Quantity - oldQuantity;

            if (quantityDiff > 0)
            {
                await _inventoryClient.ReserveStockAsync(request.ProductId, quantityDiff);
            }
            else if (quantityDiff < 0)
            {
                await _inventoryClient.ReleaseStockAsync(request.ProductId, Math.Abs(quantityDiff));
            }

            cartItem.Quantity = request.Quantity;
            _cartRepo.Update(cartItem);
            await _cartRepo.SaveChangesAsync();

            return new UpdateCartItemQuantityResponse
            {
                Message = "Quantity updated"
            };
        }
    }
}
