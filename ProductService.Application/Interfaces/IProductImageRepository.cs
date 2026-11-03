using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IProductImageRepository
    {
        Task<List<ProductImage>> GetByProductIdAsync(Guid productId);
        Task<ProductImage?> GetByIdAsync(Guid id);
        Task AddAsync(ProductImage image);
        Task UpdateAsync(ProductImage image);
        Task DeleteAsync(Guid id);
    }
}