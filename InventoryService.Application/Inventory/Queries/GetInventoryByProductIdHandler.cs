using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Inventory.Queries
{
    public class GetInventoryByProductIdHandler
        : IRequestHandler<GetInventoryByProductIdQuery, InventoryItem>
    {
        private readonly IInventoryRepository _repo;

        public GetInventoryByProductIdHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<InventoryItem> Handle(
            GetInventoryByProductIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repo.GetByProductIdAsync(request.ProductId);
        }
    }
}
