using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Persistence
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly AppDbContext _context;
        public ProductVariantRepository(AppDbContext context) { _context = context; }

        public async Task<List<ProductVariant>> GetByProductIdAsync(Guid productId)
            => await _context.ProductVariants.Where(x => x.ProductId == productId).ToListAsync();

        public async Task<ProductVariant?> GetByIdAsync(Guid id)
            => await _context.ProductVariants.FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(ProductVariant variant)
        {
            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ProductVariant variant)
        {
            _context.ProductVariants.Update(variant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var v = await _context.ProductVariants.FirstOrDefaultAsync(x => x.Id == id);
            if (v != null) { _context.ProductVariants.Remove(v); await _context.SaveChangesAsync(); }
        }
    }
}