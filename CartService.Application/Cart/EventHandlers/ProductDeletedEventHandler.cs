using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartService.Application.Interfaces;
using CartService.Application.Events;
namespace CartService.Application.Cart.EventHandlers
{
	public class ProductDeletedEventHandler
	{
		private readonly ICartRepository _cartRepository;

		public ProductDeletedEventHandler(ICartRepository cartRepository)
		{
			_cartRepository = cartRepository;
		}

        public async Task Handle(ProductDeletedEvent @event)
        {
            var items = await _cartRepository.GetByProductIdAsync(@event.Id);

            foreach (var item in items)
            {
                item.IsValid = false; // 🔥 thay vì xóa
            }

            await _cartRepository.SaveChangesAsync();
        }
    }
}
