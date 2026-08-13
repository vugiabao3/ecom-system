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
                    throw new Exception("Inventory not found");

                // 🔥 kiểm tra reserved trước
                if (inventory.Reserved < item.Quantity)
                    throw new Exception("Invalid reserved stock");

                // 💳 FLOW 2: CHUYỂN RESERVED → SOLD
                inventory.Reserved -= item.Quantity;

                // ❗ KHÔNG đụng Available nữa
                // Available đã bị trừ ở FLOW 1

                _repo.Update(inventory);
            }

            await _repo.SaveChangesAsync();
        }
    }
}
