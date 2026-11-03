using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcomSystem.Contracts.Enums;

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
            var lowerId = userId.ToLower();
            return await _db.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .Where(o => o.UserId.ToLower() == lowerId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<(List<Order> Items, int TotalCount)> GetByUserIdPagedAsync(string userId, int page, int pageSize, string? statusFilter = null)
        {
            var lowerId = userId.ToLower();
            var query = _db.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .Where(o => o.UserId.ToLower() == lowerId)
                .OrderByDescending(o => o.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                if (Enum.TryParse<OrderStatus>(statusFilter, true, out var parsedStatus))
                {
                    query = query.Where(o => o.Status == parsedStatus);
                }
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public int CountByUserIdAndStatus(string userId, string? status = null)
        {
            var lowerId = userId.ToLower();
            var query = _db.Orders.Where(o => o.UserId.ToLower() == lowerId).AsQueryable();
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
                {
                    query = query.Where(o => o.Status == parsedStatus);
                }
            }
            return query.Count();
        }

        public async Task<List<Order>> GetBySellerIdAsync(Guid sellerId)
        {
            return await _db.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .Where(o => o.Items.Any(i => i.SellerId == sellerId))
                .ToListAsync();
        }

        public async Task<List<Order>> GetByUserIdRecentAsync(string userId, int limit = 10)
        {
            var lowerId = userId.ToLower();
            return await _db.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .Where(o => o.UserId.ToLower() == lowerId)
                .OrderByDescending(o => o.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<(List<Order> Items, int TotalCount)> SearchByUserIdAsync(string userId, string searchTerm, int page, int pageSize)
        {
            var lowerId = userId.ToLower();
            var normalizedSearch = searchTerm.Trim().ToUpper();
            var query = _db.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .Where(o => o.UserId.ToLower() == lowerId)
                .Where(o => EF.Functions.Like(o.Id.ToString(), "%" + normalizedSearch + "%"))
                .OrderByDescending(o => o.CreatedAt)
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
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
