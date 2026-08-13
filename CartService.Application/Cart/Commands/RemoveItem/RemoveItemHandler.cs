using CartService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Cart.Commands.RemoveItem
{
    public class RemoveItemHandler
    : IRequestHandler<RemoveItemCommand, RemoveItemResponse>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IInventoryServiceClient _inventoryService;
        public RemoveItemHandler(
            ICartRepository cartRepository,
            ICurrentUserService currentUser,
            IInventoryServiceClient inventoryService)
        {
            _cartRepository = cartRepository;
            _currentUser = currentUser;
            _inventoryService = inventoryService;
        }

        public async Task<RemoveItemResponse> Handle(
            RemoveItemCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            // 🔥 1. tìm item
            var item = await _cartRepository.GetItemAsync(userId, request.ProductId);

            if (item == null)
                throw new Exception("Item not found in cart");

            // 🔥 2. business logic
            if (item.Quantity > 1)
            {
                item.Quantity -= 1;
                await _cartRepository.UpdateAsync(item);
            }
            else
            {
                await _cartRepository.DeleteAsync(item);
            }
            // trả stock lại inventory
            await _inventoryService.ReleaseStockAsync(
                request.ProductId,
                1);
            return new RemoveItemResponse
            {
                Message = "Item removed"
            };
        }
    }
}
