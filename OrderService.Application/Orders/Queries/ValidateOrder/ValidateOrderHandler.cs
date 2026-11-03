using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Queries.ValidateOrder
{
    public class ValidateOrderHandler : IRequestHandler<ValidateOrderQuery, ValidateResponse>
    {
        private readonly ICartServiceClient _cartClient;
        private readonly IProductServiceClient _productClient;
        private readonly IInventoryServiceClient _inventoryClient;
        private readonly IPromotionClient _promotionClient;
        private readonly ICurrentUserService _currentUser;

        public ValidateOrderHandler(
            ICartServiceClient cartClient,
            IProductServiceClient productClient,
            IInventoryServiceClient inventoryClient,
            IPromotionClient promotionClient,
            ICurrentUserService currentUser)
        {
            _cartClient = cartClient;
            _productClient = productClient;
            _inventoryClient = inventoryClient;
            _promotionClient = promotionClient;
            _currentUser = currentUser;
        }

        public async Task<ValidateResponse> Handle(ValidateOrderQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            var response = new ValidateResponse { IsValid = true };
            var selectedItems = await _cartClient.GetSelectedCart();

            if (!selectedItems.Any())
            {
                response.IsValid = false;
                response.Errors.Add("No selected cart items");
                return response;
            }

            var productIds = selectedItems.Select(x => x.ProductId).Distinct().ToList();
            var products = await _productClient.GetProductsByIds(productIds);

            foreach (var item in selectedItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                var itemValidation = new ItemValidationDto
                {
                    ProductId = item.ProductId,
                    ProductName = product?.Name ?? "[DELETED]",
                    Quantity = item.Quantity
                };

                if (product == null)
                {
                    itemValidation.IsValid = false;
                    itemValidation.Error = "Product not found";
                    response.IsValid = false;
                    response.Errors.Add($"Product {item.ProductId} not found");
                }
                else
                {
                    var inventory = await _inventoryClient.GetInventoryByProductId(item.ProductId);
                    if (inventory.Available < item.Quantity)
                    {
                        itemValidation.IsValid = false;
                        itemValidation.Error = "Insufficient stock";
                        response.IsValid = false;
                        response.Errors.Add($"Insufficient stock for {product.Name}");
                    }
                }

                response.ItemValidations.Add(itemValidation);
            }

            if (!string.IsNullOrEmpty(request.CouponCode) && response.IsValid)
            {
                var subTotal = selectedItems.Sum(i =>
                {
                    var p = products.FirstOrDefault(x => x.Id == i.ProductId);
                    return p?.Price * i.Quantity ?? 0;
                });
                var promo = await _promotionClient.Apply(request.CouponCode, subTotal, null, null);
                if (!promo.isValid)
                {
                    response.IsValid = false;
                    response.Errors.Add(promo.message ?? "Coupon invalid");
                }
            }

            return response;
        }
    }
}
