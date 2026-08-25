using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Inventory.Commands.StockAdjustment
{
    public class StockAdjustmentHandler : IRequestHandler<StockAdjustmentCommand, StockAdjustmentResponse>
    {
        private readonly IInventoryRepository _repo;

        public StockAdjustmentHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<StockAdjustmentResponse> Handle(StockAdjustmentCommand request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByProductIdAsync(request.ProductId);

            if (item == null)
            {
                item = new InventoryItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = request.ProductId,
                    Available = 0,
                    Reserved = 0,
                    Sold = 0
                };

                await _repo.AddAsync(item);
            }

            if (request.Type == "Increase")
            {
                item.Available += request.Quantity;
            }
            else if (request.Type == "Decrease")
            {
                item.Available -= request.Quantity;
                if (item.Available < 0)
                    item.Available = 0;
            }

            item.UpdatedAt = DateTime.UtcNow;
            _repo.Update(item);
            await _repo.SaveChangesAsync();

            return new StockAdjustmentResponse
            {
                ProductId = item.ProductId,
                Available = item.Available,
                Reserved = item.Reserved,
                Sold = item.Sold
            };
        }
    }
}
