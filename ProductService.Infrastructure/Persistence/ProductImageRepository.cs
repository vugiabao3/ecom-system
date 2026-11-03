using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Persistence
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _context;
        public ProductImageRepository(AppDbContext context) { _context = context; }

        public async Task<List<ProductImage>> GetByProductIdAsync(Guid productId)
            => await _context.ProductImages.Where(x => x.ProductId == productId).OrderBy(x => x.DisplayOrder).ToListAsync();

        public async Task<ProductImage?> GetByIdAsync(Guid id)
            => await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(ProductImage image)
        {
            _context.ProductImages.Add(image);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductImage image)
        {
            _context.ProductImages.Update(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var img = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == id);
            if (img != null) { _context.ProductImages.Remove(img); await _context.SaveChangesAsync(); }
        }
    }
}