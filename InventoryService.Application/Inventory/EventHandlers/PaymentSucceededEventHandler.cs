using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Application.Interfaces;
using InventoryService.Application.Events;

namespace InventoryService.Application.Inventory.EventHandlers
{
    public class PaymentSucceededEventHandler
    {
        private readonly IInventoryRepository _repo;

        public PaymentSucceededEventHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(PaymentSucceededEvent @event)
        {
            foreach (var item in @event.Items)
            {
                var inventory = await _repo.GetByProductIdAsync(item.ProductId);

                if (inventory == null)
                    continue;

                if (inventory.Reserved >= item.Quantity)
                {
                    inventory.Reserved -= item.Quantity;
                }
                else
                {
                    inventory.Available -= item.Quantity;
                    if (inventory.Available < 0) inventory.Available = 0;
                    inventory.Reserved = 0;
                }

                inventory.Sold += item.Quantity;

                _repo.Update(inventory);
            }

            await _repo.SaveChangesAsync();
        }
    }
}
