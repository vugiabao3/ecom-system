using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProductService.Domain.Entities;

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
    }
}