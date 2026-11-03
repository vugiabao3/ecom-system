using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IProductVariantRepository
    {
        Task<List<ProductVariant>> GetByProductIdAsync(Guid productId);
        Task<ProductVariant?> GetByIdAsync(Guid id);
        Task AddAsync(ProductVariant variant);
        Task UpdateAsync(ProductVariant variant);
        Task DeleteAsync(Guid id);
    }
}