using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromotionService.Domain.Entities;

namespace PromotionService.Application.Interfaces
{
    public interface IUserPointRepository
    {
        Task<UserPoint> GetByUserId(string userId);
        Task SaveChangesAsync();
        Task AddAsync(UserPoint userPoint); // 🔥 THÊM CÁI NÀY

    }
}
