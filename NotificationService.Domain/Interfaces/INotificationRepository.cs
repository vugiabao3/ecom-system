using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<List<Notification>> GetByUserIdAsync(string userId);
        Task<List<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(Guid id);
        Task MarkAsReadAsync(Guid id);
        Task SaveChangesAsync();
    }
}
