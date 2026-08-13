using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Application.Events;
using OrderService.Application.DTOs;
using OrderService.Application.Events.OrderService.Application.Events;
namespace OrderService.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private readonly ICartServiceClient _cartClient;
        private readonly IProductServiceClient _productClient;
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IEventBus _eventBus;
        private readonly IPromotionClient _promotionClient;

        public CreateOrderHandler(
            ICartServiceClient cartClient,
            IProductServiceClient productClient,
            IOrderRepository orderRepository,
            ICurrentUserService currentUser,
            IEventBus eventBus, IPromotionClient promotionClient)
        {
            _cartClient = cartClient;
            _productClient = productClient;
            _orderRepository = orderRepository;
            _currentUser = currentUser;
            _eventBus = eventBus;
            _promotionClient = promotionClient;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unauthorized");

            // 🔥 1. lấy cart
            var cart = await _cartClient.GetCart();

            if (!cart.Items.Any())
                throw new Exception("Cart empty");

            // 🔥 2. validate product
            var productIds = cart.Items.Select(x => x.ProductId).ToList();
            var products = await _productClient.GetProductsByIds(productIds);

            // 1. tạo order trước
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Address = request.Address,
                Phone = request.Phone ?? "N/A",
                ReceiverName = request.ReceiverName ?? "Unknown",
                Status = "PENDING",
                Items = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in cart.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);

                if (product == null)
                    throw new Exception("Product invalid");

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = item.Quantity,
                    OrderId = order.Id   // 🔥 FIX FK (rất quan trọng)
                };

                total += product.Price * item.Quantity;
                order.Items.Add(orderItem);
            }

            decimal subTotal = total;
            decimal discount = 0;
            decimal finalPrice = total;

            if (!string.IsNullOrEmpty(request.CouponCode))
            {
                var promo = await _promotionClient.Apply(request.CouponCode, subTotal);
                Console.WriteLine($"🔥 PROMO DISCOUNT: {promo.discountAmount}");
                Console.WriteLine($"🔥 PROMO FINAL: {promo.finalAmount}");
                if (promo.isValid)
                {
                    discount = promo.discountAmount;   // 🔥 lấy từ promotion
                    finalPrice = promo.finalAmount;    // 🔥 hoặc subTotal - discount
                }
            }
            order.SubTotal = subTotal;
            order.Discount = discount;
            order.TotalPrice = finalPrice;


            // 🔥 3. save DB
            await _orderRepository.AddAsync(order);

            // 🔥 4. clear cart
            await _cartClient.ClearCart();

            // 🔥 5. publish event
            var eventItems = order.Items.Select(x => new DTOs.OrderItemDto
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList();
            await _eventBus.PublishAsync("OrderCreated", new OrderCreatedEvent
            {
                OrderId = order.Id,
                Items = eventItems,
                TotalAmount = order.TotalPrice,
                UserId = userId
            });
            return new CreateOrderResponse
            {
                OrderId = order.Id,
                SubTotal = subTotal,
                Discount = discount,
                TotalPrice = finalPrice
            };
        }
    }
}
