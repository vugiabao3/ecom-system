using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order> GetByIdAsync(Guid id);
        Task<List<Order>> GetByUserIdAsync(string userId);
        Task<List<Order>> GetBySellerIdAsync(Guid sellerId);
        Task<(List<Order> Items, int TotalCount)> GetByUserIdPagedAsync(string userId, int page, int pageSize, string? statusFilter = null);
        Task<List<Order>> GetByUserIdRecentAsync(string userId, int limit = 10);
        Task<(List<Order> Items, int TotalCount)> SearchByUserIdAsync(string userId, string searchTerm, int page, int pageSize);
        int CountByUserIdAndStatus(string userId, string? status = null);
        void Update(Order order);
        Task SaveChangesAsync();
    }
}
