using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _db.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetByUserIdAsync(string userId)
        {
            return await _db.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetBySellerIdAsync(Guid sellerId)
        {
            return await _db.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .Where(o => o.Items.Any(i => i.SellerId == sellerId))
                .ToListAsync();
        }

        public void Update(Order order)
        {
            _db.Orders.Update(order);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
