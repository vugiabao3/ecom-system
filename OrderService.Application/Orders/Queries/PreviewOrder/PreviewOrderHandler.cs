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

namespace OrderService.Application.Orders.Queries.PreviewOrder
{
    public class PreviewOrderHandler : IRequestHandler<PreviewOrderQuery, PreviewResponse>
    {
        private readonly ICartServiceClient _cartClient;
        private readonly IProductServiceClient _productClient;
        private readonly IInventoryServiceClient _inventoryClient;
        private readonly IPromotionClient _promotionClient;
        private readonly ICheckoutSessionRepository _sessionRepository;
        private readonly ICurrentUserService _currentUser;

        public PreviewOrderHandler(
            ICartServiceClient cartClient,
            IProductServiceClient productClient,
            IInventoryServiceClient inventoryClient,
            IPromotionClient promotionClient,
            ICheckoutSessionRepository sessionRepository,
            ICurrentUserService currentUser)
        {
            _cartClient = cartClient;
            _productClient = productClient;
            _inventoryClient = inventoryClient;
            _promotionClient = promotionClient;
            _sessionRepository = sessionRepository;
            _currentUser = currentUser;
        }

        public async Task<PreviewResponse> Handle(PreviewOrderQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            var selectedItems = await _cartClient.GetSelectedCart();
            var response = new PreviewResponse();

            if (!selectedItems.Any())
            {
                response.Errors.Add("No selected cart items");
                return response;
            }

            var productIds = selectedItems.Select(x => x.ProductId).Distinct().ToList();
            var products = await _productClient.GetProductsByIds(productIds);

            decimal subTotal = 0;
            foreach (var item in selectedItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                var itemDto = new PreviewItemDto
                {
                    ProductId = item.ProductId,
                    SellerId = item.SellerId,
                    ProductName = product?.Name ?? "[DELETED]",
                    Sku = product?.Sku,
                    VariantName = product?.VariantName,
                    ImageUrl = product?.ImageUrl,
                    Price = product?.Price ?? 0,
                    Quantity = item.Quantity
                };

                if (product == null)
                {
                    itemDto.IsValid = false;
                    itemDto.Error = "Product not found";
                    response.Errors.Add($"Product {item.ProductId} not found");
                }
                else
                {
                    var inventory = await _inventoryClient.GetInventoryByProductId(item.ProductId);
                    if (inventory.Available < item.Quantity)
                    {
                        itemDto.IsValid = false;
                        itemDto.Error = "Insufficient stock";
                        response.Errors.Add($"Insufficient stock for {product.Name}");
                    }
                    else
                    {
                        itemDto.IsValid = true;
                        subTotal += product.Price * item.Quantity;
                    }
                }

                response.Items.Add(itemDto);
            }

            response.IsValid = !response.Errors.Any();
            response.SubTotal = subTotal;
            response.ShippingFee = subTotal < 200000 ? 30000 : 0;
            response.Discount = 0;
            response.TotalPrice = subTotal + response.ShippingFee;

            if (!string.IsNullOrEmpty(request.CouponCode) && response.IsValid)
            {
                var promo = await _promotionClient.Apply(request.CouponCode, subTotal, null, null);
                if (promo.isValid)
                {
                    response.Discount = promo.discountAmount;
                    response.TotalPrice = subTotal + response.ShippingFee - response.Discount;
                    if (response.TotalPrice < 0) response.TotalPrice = 0;
                }
                else
                {
                    response.Errors.Add(promo.message ?? "Coupon invalid");
                }
            }

            // Persist a checkout session for this preview so it can be retrieved and confirmed later.
            var session = new CheckoutSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = CheckoutSessionStatus.Pending,
                SubTotal = response.SubTotal,
                ShippingFee = response.ShippingFee,
                Discount = response.Discount,
                TotalPrice = response.TotalPrice,
                CouponCode = request.CouponCode,
                Address = request.Address ?? string.Empty,
                Phone = request.Phone ?? string.Empty,
                ReceiverName = request.ReceiverName ?? string.Empty,
                PaymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod, true),
                Items = response.Items.Where(i => i.IsValid).Select(i => new CheckoutSessionItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    SellerId = i.SellerId,
                    ProductName = i.ProductName,
                    Sku = i.Sku,
                    VariantName = i.VariantName,
                    ImageUrl = i.ImageUrl,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
            await _sessionRepository.AddAsync(session);
            await _sessionRepository.SaveChangesAsync();

            response.CheckoutId = session.Id;
            return response;
        }
    }
}
