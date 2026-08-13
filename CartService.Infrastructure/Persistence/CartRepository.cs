using CartService.Application.Interfaces;
using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CartService.Infrastructure.Persistence
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
        }
        public async Task<List<CartItem>> GetByUserIdAsync(string userId)
        {
            return await _context.CartItems
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        public async Task<CartItem> GetItemAsync(string userId, Guid productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
        }

        public async Task UpdateAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByUserIdAsync(string userId)
        {
            var items = await _context.CartItems
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _context.CartItems.RemoveRange(items);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductInfoAsync(Guid productId, string name, decimal price)
        {
            var items = await _context.CartItems
                .Where(x => x.ProductId == productId)
                .ToListAsync();

            foreach (var item in items)
            {
                item.ProductName = name;
                item.PriceSnapshot = price;
            }

            await _context.SaveChangesAsync();
        }
        public async Task<List<CartItem>> GetByProductIdAsync(Guid productId)
        {
            return await _context.CartItems
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }

        public async Task RemoveAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Update(CartItem item)
        {
            _context.CartItems.Update(item);
        }
    }
}
