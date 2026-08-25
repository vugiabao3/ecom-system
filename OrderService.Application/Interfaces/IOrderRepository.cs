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
        void Update(Order order);
        Task SaveChangesAsync();
    }
}
