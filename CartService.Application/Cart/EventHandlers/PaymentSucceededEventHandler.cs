using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartService.Application.Interfaces;
using PaymentService.Application.Events;

namespace CartService.Application.Cart.EventHandlers
{
    public class PaymentSucceededEventHandler
    {
        private readonly ICartRepository _cartRepo;

        public PaymentSucceededEventHandler(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task Handle(PaymentSucceededEvent @event)
        {
            if (string.IsNullOrEmpty(@event?.UserId))
                return;

            var items = await _cartRepo.GetByUserIdAsync(@event.UserId);
            if (items == null || !items.Any())
                return;

            foreach (var item in items)
            {
                await _cartRepo.DeleteAsync(item);
            }

            await _cartRepo.SaveChangesAsync();
        }
    }
}
