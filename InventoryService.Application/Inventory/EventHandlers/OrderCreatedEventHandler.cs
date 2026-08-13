using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Application.Events;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Inventory.EventHandlers
{
    public class OrderCreatedEventHandler
    {
        private readonly IInventoryRepository _repo;
        private readonly IEventBus _eventBus;

        public OrderCreatedEventHandler(
            IInventoryRepository repo,
            IEventBus eventBus)
        {
            _repo = repo;
            _eventBus = eventBus;
        }

        public async Task Handle(OrderCreatedEvent @event)
        {
            var failed = new List<Guid>();
            if (@event?.Items == null || !@event.Items.Any())
            {
                Console.WriteLine("❌ Items null hoặc rỗng");
                return;
            }
            foreach (var item in @event.Items)
            {
                var stock = await _repo.GetByProductIdAsync(item.ProductId);

                if (stock == null || stock.Available < item.Quantity)
                {
                    failed.Add(item.ProductId);
                    continue;
                }

                // 🔥 reserve
                stock.Available -= item.Quantity;
                stock.Reserved += item.Quantity;

                _repo.Update(stock);
            }

            await _repo.SaveChangesAsync();

            // 🔥 publish result
            if (failed.Any())
            {
                await _eventBus.PublishAsync(new StockRejectedEvent
                {
                    OrderId = @event.OrderId,
                    FailedProducts = failed
                });
            }
            else
            {
                await _eventBus.PublishAsync(new StockReservedEvent
                {
                    OrderId = @event.OrderId
                });
            }
            Console.WriteLine("🔥 RESERVE STOCK RUNNING");
        }
    }
}