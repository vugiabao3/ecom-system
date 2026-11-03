using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByIdIncludingDeletedAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
        Task<List<User>> GetAllAsync(int page, int pageSize);
        Task<int> CountAsync();
        Task<List<User>> SearchAsync(string keyword, int page, int pageSize);
        Task<int> CountSearchAsync(string keyword);
        Task BlockUserAsync(Guid userId);
        Task UnblockUserAsync(Guid userId);
        Task SoftDeleteUserAsync(Guid userId);
        Task RestoreUserAsync(Guid userId);
        Task UpdateStatusAsync(Guid userId, string status);
        Task AddActivityLogAsync(UserActivityLog log);
        Task<List<UserActivityLog>> GetUserActivityAsync(Guid userId);
        Task<List<UserAddress>> GetUserAddressesAsync(Guid userId);
        Task<UserAddress?> GetUserAddressByIdAsync(Guid addressId);
        Task AddUserAddressAsync(UserAddress address);
        Task UpdateUserAddressAsync(UserAddress address);
        Task DeleteUserAddressAsync(UserAddress address);
        Task SetDefaultAddressAsync(Guid userId, Guid addressId);
        Task<List<UserSession>> GetUserDevicesAsync(Guid userId);
        Task<UserSession?> GetUserDeviceByIdAsync(Guid deviceId);
        Task RemoveUserDeviceAsync(UserSession session);
        Task LogoutAllDevicesAsync(Guid userId);
        Task CreateUserDeviceAsync(UserSession session);
        Task SaveChangesAsync();
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetActiveUsersCountAsync();
        Task<int> GetBlockedUsersCountAsync();
        Task<int> GetDeletedUsersCountAsync();
    }
}
