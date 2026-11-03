using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Queries.CalculateOrder
{
    public class CalculateOrderHandler : IRequestHandler<CalculateOrderQuery, CalculateResponse>
    {
        private readonly ICartServiceClient _cartClient;
        private readonly IProductServiceClient _productClient;
        private readonly IPromotionClient _promotionClient;
        private readonly ICurrentUserService _currentUser;

        public CalculateOrderHandler(
            ICartServiceClient cartClient,
            IProductServiceClient productClient,
            IPromotionClient promotionClient,
            ICurrentUserService currentUser)
        {
            _cartClient = cartClient;
            _productClient = productClient;
            _promotionClient = promotionClient;
            _currentUser = currentUser;
        }

        public async Task<CalculateResponse> Handle(CalculateOrderQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            var selectedItems = await _cartClient.GetSelectedCart();
            var response = new CalculateResponse();

            if (!selectedItems.Any())
                return response;

            var productIds = selectedItems.Select(x => x.ProductId).Distinct().ToList();
            var products = await _productClient.GetProductsByIds(productIds);

            decimal subTotal = 0;
            foreach (var item in selectedItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                    subTotal += product.Price * item.Quantity;
            }

            decimal shippingFee = subTotal < 200000 ? 30000 : 0;
            decimal discount = 0;

            if (!string.IsNullOrEmpty(request.CouponCode))
            {
                var promo = await _promotionClient.Apply(request.CouponCode, subTotal, null, null);
                if (promo.isValid)
                    discount = promo.discountAmount;
            }

            decimal total = subTotal + shippingFee - discount;
            if (total < 0) total = 0;

            response.SubTotal = subTotal;
            response.ShippingFee = shippingFee;
            response.Discount = discount;
            response.TotalPrice = total;
            return response;
        }
    }
}
