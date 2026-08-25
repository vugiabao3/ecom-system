using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Application.Interfaces;
using InventoryService.Application.Events;

namespace InventoryService.Application.Inventory.EventHandlers
{
    public class OrderReturnedEventHandler
    {
        private readonly IInventoryRepository _repo;

        public OrderReturnedEventHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(OrderReturnedEvent @event)
        {
            foreach (var item in @event.Items)
            {
                var inventory = await _repo.GetByProductIdAsync(item.ProductId);

                if (inventory == null)
                    continue;

                inventory.Sold -= item.Quantity;
                inventory.Available += item.Quantity;

                if (inventory.Sold < 0)
                    inventory.Sold = 0;
                if (inventory.Available < 0)
                    inventory.Available = 0;

                _repo.Update(inventory);
            }

            await _repo.SaveChangesAsync();
        }
    }
}
