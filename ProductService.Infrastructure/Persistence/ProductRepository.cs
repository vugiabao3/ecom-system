using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTO;
using ProductService.Application.Interfaces;
using ProductService.Application.Products.Queries.GetAllProducts;
using ProductService.Application.Products.Queries.SearchProduct;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task<Product> GetByIdIncludeDeletedAsync(Guid id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<(List<ProductDto>, int)> SearchAsync(SearchProductsQuery query)
        {
            var products = _context.Products
    .Include(p => p.Category)
    .Where(p => !p.IsDeleted);

            // 🔥 filter
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim().ToLower();

                products = products.Where(p => p.Name.ToLower().Contains(keyword));
            }

            if (query.CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == query.CategoryId);
            }

            if (query.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= query.MinPrice);
            }

            if (query.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= query.MaxPrice);
            }

            // 🔥 count trước pagination
            var total = await products.CountAsync();

            // 🔥 paging
            var items = await products
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            return (items, total);
        }
        public async Task<List<Product>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Products
                 .Include(x => x.Category) // ⭐ BẮT BUỘC THÊM
                .Where(p => ids.Contains(p.Id) && !p.IsDeleted)
                .ToListAsync();
        }
        public async Task<(List<ProductDto> Items, int TotalCount)> GetAllAsync(GetAllProductsQuery query)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted);

            // 🔥 FILTER CATEGORY
            if (query.CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == query.CategoryId);
            }

            // 🔥 FILTER PRICE
            if (query.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= query.MinPrice);
            }

            if (query.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= query.MaxPrice);
            }

            // 🔥 SORT
            products = query.SortBy switch
            {
                "price" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                _ => products.OrderByDescending(p => p.Id)
            };

            // 🔥 COUNT
            var total = await products.CountAsync();

            // 🔥 PAGING
            var items = await products
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            return (items, total);
        }
    }
}