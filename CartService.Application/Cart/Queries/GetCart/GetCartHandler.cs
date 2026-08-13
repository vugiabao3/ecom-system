using CartService.Application.DTOs;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Cart.Queries.GetCart
{
    public class GetCartHandler : IRequestHandler<GetCartQuery, GetCartResponse>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductServiceClient _productService;
        private readonly ICurrentUserService _currentUser;

        public GetCartHandler(
            ICartRepository cartRepository,
            IProductServiceClient productService,
            ICurrentUserService currentUser)
        {
            _cartRepository = cartRepository;
            _productService = productService;
            _currentUser = currentUser;
        }

        public async Task<GetCartResponse> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            // 🔥 1. Lấy userId
            var userId = _currentUser.UserId;

            // 🔥 2. Lấy cart items
            var cartItems = await _cartRepository.GetByUserIdAsync(userId)
                  ?? new List<CartItem>();

            cartItems = cartItems.Where(x => x.IsValid).ToList();

            // 🔥 3. gọi ProductService (batch)
            var productIds = cartItems.Select(x => x.ProductId).ToList();

            var products = productIds.Count > 0
                ? await _productService.GetProductsByIds(productIds)
                : new List<ProductDto>();

            // 🔥 4. map + enrich
            var items = cartItems.Select(ci =>
            {
                var product = products?.FirstOrDefault(p => p.Id == ci.ProductId);

                return new CartItemDto
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    ProductName = product?.Name ?? "[DELETED]",
                    Price = product?.Price ?? ci.PriceSnapshot, // fallback snapshot
                    // 🔥 FIX Ở ĐÂY
    ImageUrl = product?.ImageUrl ?? ""
                };
            }).ToList();

            // 🔥 5. tính total
            var total = items.Sum(x => x.Price * x.Quantity);

            return new GetCartResponse
            {
                Items = items,
                TotalPrice = total
            };
        }
    }
}
