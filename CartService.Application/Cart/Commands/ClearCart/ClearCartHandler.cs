using CartService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Cart.Commands.ClearCart
{
    public class ClearCartHandler
    : IRequestHandler<ClearCartCommand, ClearCartResponse>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IInventoryServiceClient _inventoryService;

        public ClearCartHandler(
            ICartRepository cartRepository,
            ICurrentUserService currentUser,IInventoryServiceClient inventoryService)
        {
            _cartRepository = cartRepository;
            _currentUser = currentUser;
            _inventoryService = inventoryService;
        }

        public async Task<ClearCartResponse> Handle(
            ClearCartCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var items = await _cartRepository.GetByUserIdAsync(userId);

            foreach (var item in items)
            {
                await _inventoryService.ReleaseStockAsync(
                    item.ProductId,
                    item.Quantity);
            }

            // 🔥 xóa toàn bộ cart của user
            await _cartRepository.DeleteByUserIdAsync(userId);

            return new ClearCartResponse
            {
                Message = "Cart cleared"
            };
        }
    }
}
