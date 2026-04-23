using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Persistence
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Product>, int)> GetAllAsync(int page, int size, int? category, string? sort)
        {
            var query = _context.Products
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (category.HasValue)
                query = query.Where(p => p.CategoryId == category);

            if (sort == "price_asc")
                query = query.OrderBy(p => p.Price);
            else if (sort == "price_desc")
                query = query.OrderByDescending(p => p.Price);
            else
                query = query.OrderByDescending(p => p.Id);

            var total = await query.CountAsync();

            var products = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (products, total);
        }

        public async Task<Product> GetProductDetailAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

    }
}