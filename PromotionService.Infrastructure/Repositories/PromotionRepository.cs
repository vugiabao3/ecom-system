using PromotionService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Microsoft.EntityFrameworkCore;
using PromotionService.Application.Interfaces;
using PromotionService.Domain.Entities;

namespace PromotionService.Infrastructure.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly AppDbContext _context;

        public PromotionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Promotion promotion)
        {
            await _context.Promotions.AddAsync(promotion);

            await _context.SaveChangesAsync();
        }
        public async Task<Promotion?> GetByCodeAsync(string code)
        {
            return await _context.Promotions
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task UpdateAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);

            await _context.SaveChangesAsync();
        }

        public async Task<Promotion?> GetByIdAsync(Guid id)
        {
            return await _context.Promotions
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task DeleteAsync(
    Promotion promotion)
        {
            _context.Promotions.Remove(promotion);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Promotion>> GetAllAsync()
        {
            return await _context.Promotions
                .ToListAsync();
        }

    }
}