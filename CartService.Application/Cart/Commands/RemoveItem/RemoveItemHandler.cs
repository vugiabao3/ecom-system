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
            var item = await _cartRepository.GetItemByIdAsync(userId, request.ItemId);

            if (item == null)
                throw new Exception("Item not found in cart");

            await _cartRepository.DeleteAsync(item);

            await _inventoryService.ReleaseStockAsync(
                item.ProductId,
                item.Quantity);

            return new RemoveItemResponse
            {
                Message = "Item removed"
            };
        }
    }
}
