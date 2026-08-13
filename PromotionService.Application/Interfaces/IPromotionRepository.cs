using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PromotionService.Domain.Entities;

namespace PromotionService.Application.Interfaces
{

    public interface IPromotionRepository
    {
        Task<Promotion?> GetByCodeAsync(string code);
        Task AddAsync(Promotion promotion);
        Task UpdateAsync(Promotion promotion);
        Task<Promotion?> GetByIdAsync(Guid id);

        Task DeleteAsync(Promotion promotion);
        Task<List<Promotion>> GetAllAsync();
    }
}
