using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces
{
    public interface IBrandRepository
    {
        Task<Brand> GetByIdAsync(Guid id);
        Task<List<Brand>> GetAllAsync();
        Task AddAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(Guid id);
    }
}
