using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using PromotionService.Application.Interfaces;
using PromotionService.Domain.Entities;
using PromotionService.Infrastructure.Persistence;

namespace PromotionService.Infrastructure.Repositories
{
    public class UserPointRepository : IUserPointRepository
    {
        private readonly AppDbContext _context;

        public UserPointRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserPoint> GetByUserId(string userId)
        {
            return await _context.UserPoints
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(UserPoint userPoint)
        {
            await _context.UserPoints.AddAsync(userPoint); // 🔥 IMPORTANT
        }
    }
}