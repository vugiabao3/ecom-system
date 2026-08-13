using CartService.Application.Events;
using CartService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Cart.EventHandlers
{
    public class ProductUpdatedEventHandler
    {
        private readonly ICartRepository _cartRepository;

        public ProductUpdatedEventHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(ProductUpdatedEvent @event)
        {
            // 🔥 update product info trong cart
            await _cartRepository.UpdateProductInfoAsync(
                @event.Id,
                @event.Name,
                @event.Price
            );
        }
    }
}
