using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartService.Domain.Entities;
namespace CartService.Application.Interfaces
{
    public interface ICartRepository
    {
        Task AddAsync(CartItem item);
        Task<List<CartItem>> GetByUserIdAsync(string userId);
        Task<CartItem> GetItemAsync(string userId, Guid productId);
        Task UpdateAsync(CartItem item);
        Task DeleteAsync(CartItem item);

        Task DeleteByUserIdAsync(string userId);
        Task UpdateProductInfoAsync(Guid productId, string name, decimal price);
        Task<List<CartItem>> GetByProductIdAsync(Guid productId);
        Task RemoveAsync(CartItem item);
        Task SaveChangesAsync();
        void Update(CartItem item);
    }
}
