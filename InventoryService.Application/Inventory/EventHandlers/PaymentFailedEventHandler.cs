using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Application.Interfaces;
using InventoryService.Application.Events;
using System;
using System.Threading.Tasks;

namespace InventoryService.Application.Inventory.EventHandlers
{
    public class PaymentFailedEventHandler
    {
        private readonly IInventoryRepository _repo;

        public PaymentFailedEventHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(PaymentFailedEvent @event)
        {
            foreach (var item in @event.Items)
            {
                var inventory = await _repo.GetByProductIdAsync(item.ProductId);

                if (inventory == null)
                    continue;

                // 🔥 ROLLBACK LOGIC
                inventory.Reserved -= item.Quantity;
                inventory.Available += item.Quantity;

                if (inventory.Reserved < 0)
                    inventory.Reserved = 0;

                _repo.Update(inventory);
            }

            await _repo.SaveChangesAsync();
        }
    }
}