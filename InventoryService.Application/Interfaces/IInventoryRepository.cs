using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InventoryService.Domain.Entities;

namespace InventoryService.Application.Interfaces
{
    public interface IInventoryRepository
    {
        Task<InventoryItem> GetByProductIdAsync(Guid productId);

        void Update(InventoryItem item);
        Task AddAsync(InventoryItem item);
        Task SaveChangesAsync();

    }
}
