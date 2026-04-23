using AuthService.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IAuthUserRepository
    {
        Task<AuthUser?> GetByEmailAsync(string email);
        Task<AuthUser?> GetByIdAsync(Guid id);

        Task CreateAsync(AuthUser user);

        Task UpdatePasswordAsync(Guid userId, string passwordHash);

        Task UpdateStatusAsync(Guid userId, string status);

        Task SaveChangesAsync();
        Task LogoutAllDevicesAsync(Guid userId);
    }
}
