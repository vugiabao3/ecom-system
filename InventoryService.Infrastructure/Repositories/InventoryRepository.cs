using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.Persistence;

namespace InventoryService.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _db;

        public InventoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<InventoryItem> GetByProductIdAsync(Guid productId)
        {
            return await _db.InventoryItems
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public void Update(InventoryItem item)
        {
            _db.InventoryItems.Update(item);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
        public async Task AddAsync(InventoryItem item)
        {
            await _db.InventoryItems.AddAsync(item);
        }
    }
}