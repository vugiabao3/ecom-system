using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Application.Events;
using EcomSystem.Contracts.Enums;

namespace OrderService.Application.Orders.Commands.Checkout
{
    public class CheckoutHandler : IRequestHandler<CheckoutCommand, DTOs.CheckoutResponse>
    {
        private readonly ICartServiceClient _cartClient;
        private readonly IProductServiceClient _productClient;
        private readonly IInventoryServiceClient _inventoryClient;
        private readonly IPromotionClient _promotionClient;
        private readonly IOrderRepository _orderRepository;
        private readonly ICheckoutSessionRepository _sessionRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IEventBus _eventBus;

        public CheckoutHandler(
            ICartServiceClient cartClient,
            IProductServiceClient productClient,
            IInventoryServiceClient inventoryClient,
            IPromotionClient promotionClient,
            IOrderRepository orderRepository,
            ICheckoutSessionRepository sessionRepository,
            ICurrentUserService currentUser,
            IEventBus eventBus)
        {
            _cartClient = cartClient;
            _productClient = productClient;
            _inventoryClient = inventoryClient;
            _promotionClient = promotionClient;
            _orderRepository = orderRepository;
            _sessionRepository = sessionRepository;
            _currentUser = currentUser;
            _eventBus = eventBus;
        }

        public async Task<DTOs.CheckoutResponse> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            // Idempotency: if a completed order already exists for this key, return it.
            if (!string.IsNullOrEmpty(request.IdempotencyKey))
            {
                var existing = await _sessionRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey);
                if (existing != null && existing.Status == CheckoutSessionStatus.Completed && existing.OrderId.HasValue)
                {
                    var existingOrder = await _orderRepository.GetByIdAsync(existing.OrderId.Value);
                    if (existingOrder != null)
                    {
                        return new DTOs.CheckoutResponse
                        {
                            OrderId = existingOrder.Id,
                            CheckoutId = existing.Id,
                            SubTotal = existingOrder.SubTotal,
                            Discount = existingOrder.Discount,
                            ShippingFee = existingOrder.ShippingFee,
                            TotalPrice = existingOrder.TotalPrice,
                            PaymentMethod = existingOrder.PaymentMethod.ToString(),
                            Status = existingOrder.Status.ToString()
                        };
                    }
                }
            }

            CheckoutSession session = null;

            // If a checkout session id is provided, load and validate it.
            if (request.CheckoutId.HasValue)
            {
                session = await _sessionRepository.GetByIdAsync(request.CheckoutId.Value);
                if (session == null)
                    throw new Exception("Checkout session not found");
                if (session.UserId != userId)
                    throw new Exception("Forbidden: not your session");
                if (session.Status == CheckoutSessionStatus.Completed)
                    throw new Exception("Checkout session already completed");
            }

            var selectedItems = await _cartClient.GetSelectedCart();
            if (!selectedItems.Any())
                throw new Exception("No selected cart items");

            var productIds = selectedItems.Select(x => x.ProductId).Distinct().ToList();
            var products = await _productClient.GetProductsByIds(productIds);

            // Validate products and stock (never trust frontend prices).
            decimal subTotal = 0;
            var orderItems = new List<OrderItem>();
            var sessionItems = new List<CheckoutSessionItem>();

            foreach (var item in selectedItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                    throw new Exception($"Product {item.ProductId} not found");

                // Stock check via InventoryService (authoritative).
                var inventory = await _inventoryClient.GetInventoryByProductId(item.ProductId);
                if (inventory.Available < item.Quantity)
                    throw new Exception($"Insufficient stock for product {product.Name}");

                subTotal += product.Price * item.Quantity;

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    SellerId = product.SellerId,
                    ProductName = product.Name,
                    Sku = product.Sku,
                    VariantName = product.VariantName,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = item.Quantity
                };
                orderItems.Add(orderItem);

                var sessionItem = new CheckoutSessionItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    SellerId = product.SellerId,
                    ProductName = product.Name,
                    Sku = product.Sku,
                    VariantName = product.VariantName,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = item.Quantity
                };
                sessionItems.Add(sessionItem);
            }

            // Shipping fee rule.
            decimal shippingFee = subTotal < 200000 ? 30000 : 0;
            decimal discount = 0;

            if (!string.IsNullOrEmpty(request.CouponCode))
            {
                var promo = await _promotionClient.Apply(request.CouponCode, subTotal, null, null);
                if (promo.isValid)
                {
                    discount = promo.discountAmount;
                }
            }

            decimal finalPrice = subTotal + shippingFee - discount;
            if (finalPrice < 0) finalPrice = 0;

            // Reserve stock for all items.
            foreach (var item in selectedItems)
            {
                await _inventoryClient.ReserveStock(item.ProductId, item.Quantity);
            }

            PaymentMethod paymentMethod;
            try
            {
                paymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod, true);
            }
            catch
            {
                paymentMethod = PaymentMethod.QR;
            }

            // Create or reuse checkout session.
            if (session == null)
            {
                session = new CheckoutSession
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    IdempotencyKey = request.IdempotencyKey,
                    Status = CheckoutSessionStatus.Pending,
                    SubTotal = subTotal,
                    ShippingFee = shippingFee,
                    Discount = discount,
                    TotalPrice = finalPrice,
                    CouponCode = request.CouponCode,
                    Address = request.Address,
                    Phone = request.Phone,
                    ReceiverName = request.ReceiverName,
                    PaymentMethod = paymentMethod,
                    Items = sessionItems,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };
                await _sessionRepository.AddAsync(session);
            }
            else
            {
                session.SubTotal = subTotal;
                session.ShippingFee = shippingFee;
                session.Discount = discount;
                session.TotalPrice = finalPrice;
                session.CouponCode = request.CouponCode;
                session.Address = request.Address;
                session.Phone = request.Phone;
                session.ReceiverName = request.ReceiverName;
                session.PaymentMethod = paymentMethod;
                session.Items = sessionItems;
                _sessionRepository.Update(session);
            }

            // Create the order with immutable snapshots.
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IdempotencyKey = request.IdempotencyKey,
                Address = request.Address,
                Phone = request.Phone ?? "N/A",
                ReceiverName = request.ReceiverName ?? "Unknown",
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = paymentMethod,
                SubTotal = subTotal,
                ShippingFee = shippingFee,
                Discount = discount,
                TotalPrice = finalPrice,
                Items = orderItems,
                StatusHistory = new List<OrderStatusHistory>
                {
                    new OrderStatusHistory
                    {
                        Id = Guid.NewGuid(),
                        Status = OrderStatus.Pending.ToString(),
                        Note = "Order created",
                        CreatedAt = DateTime.UtcNow
                    }
                }
            };

            await _orderRepository.AddAsync(order);

            // Fix history order id and link session to order.
            order.StatusHistory.First().OrderId = order.Id;
            _orderRepository.Update(order);

            session.OrderId = order.Id;
            session.Status = CheckoutSessionStatus.Completed;
            _sessionRepository.Update(session);

            await _orderRepository.SaveChangesAsync();
            await _sessionRepository.SaveChangesAsync();

            // Publish OrderCreated event.
            var eventItems = order.Items.Select(x => new DTOs.OrderItemDto
            {
                ProductId = x.ProductId,
                SellerId = x.SellerId,
                Quantity = x.Quantity
            }).ToList();
            await _eventBus.PublishAsync("OrderCreated", new OrderCreatedEvent
            {
                OrderId = order.Id,
                Items = eventItems,
                TotalAmount = order.TotalPrice,
                UserId = userId,
                SubTotal = subTotal,
                ShippingFee = shippingFee,
                PaymentMethod = paymentMethod.ToString()
            });

            // Clear only the selected cart items that were purchased.
            await _cartClient.ClearSelectedCart();

            return new DTOs.CheckoutResponse
            {
                OrderId = order.Id,
                CheckoutId = session.Id,
                SubTotal = subTotal,
                Discount = discount,
                ShippingFee = shippingFee,
                TotalPrice = finalPrice,
                PaymentMethod = paymentMethod.ToString(),
                Status = order.Status.ToString()
            };
        }
    }
}
