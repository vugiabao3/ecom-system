using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;
namespace CartService.Application.Cart.Commands.AddToCart
{
    public class AddToCartHandler : IRequestHandler<AddToCartCommand, AddToCartResponse>
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductServiceClient _productClient;
        private readonly ICurrentUserService _currentUser;
        private readonly IInventoryServiceClient _inventoryClient;
        public AddToCartHandler(
            ICartRepository cartRepo,
            IProductServiceClient productClient,
            ICurrentUserService currentUser,
            IInventoryServiceClient inventoryClient)
        {
            _cartRepo = cartRepo;
            _productClient = productClient;
            _currentUser = currentUser;
            _inventoryClient = inventoryClient;
        }

        public async Task<AddToCartResponse> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            // 🔥 1. lấy userId từ JWT
            var userId = _currentUser.UserId;

            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            // 🔥 2. call ProductService
            var product = await _productClient.GetProductById(request.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            // 🔥 3. tạo cart item
            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = product.Id,
                ProductName = product.Name, // 🔥 THÊM DÒNG NÀY
                Quantity = request.Quantity,
                PriceSnapshot = product.Price
            };
            await _inventoryClient.ReserveStockAsync(
    request.ProductId,
    request.Quantity
);
            // 🔥 4. save DB
            await _cartRepo.AddAsync(cartItem);

            return new AddToCartResponse
            {
                Message = "Added to cart"
            };
        }
    }
}
