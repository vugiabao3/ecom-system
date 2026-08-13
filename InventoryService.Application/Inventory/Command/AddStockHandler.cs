using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Inventory.Commands.AddStock
{
    public class AddStockHandler : IRequestHandler<AddStockCommand, AddStockResponse>
    {
        private readonly IInventoryRepository _repo;

        public AddStockHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<AddStockResponse> Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByProductIdAsync(request.ProductId);

            if (item == null)
            {
                // 🔥 tạo mới
                item = new InventoryItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = request.ProductId,
                    Available = request.Quantity,
                    Reserved = 0
                };

                await _repo.AddAsync(item);
            }
            else
            {
                // 🔥 cộng thêm
                item.Available += request.Quantity;
                _repo.Update(item);
            }

            await _repo.SaveChangesAsync();

            return new AddStockResponse
            {
                ProductId = item.ProductId,
                Available = item.Available,
                Reserved = item.Reserved
            };
        }
    }
}