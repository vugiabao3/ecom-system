using ProductService.Application.DTO;
using ProductService.Application.Products.Queries.GetAllProducts;
using ProductService.Application.Products.Queries.SearchProduct;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<(List<Product>, int)> GetAllAsync(
            int page,
            int size,
            int? category,
            string? sort);

        Task<Product> GetProductDetailAsync(Guid id);
        Task AddAsync(Product product);

        Task<Product> GetByIdAsync(Guid id);
        Task UpdateAsync(Product product);
        Task<Product> GetByIdIncludeDeletedAsync(Guid id);
        Task<List<Product>> GetByIdsAsync(List<Guid> ids);
        Task<(List<ProductDto> Items, int TotalCount)> SearchAsync(SearchProductsQuery query);
        Task<(List<ProductDto> Items, int TotalCount)> GetAllAsync(GetAllProductsQuery query);
    }
}