using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByIdIncludingDeletedAsync(Guid id)
        {
            return await _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
       
        
        public async Task<List<User>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> CountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<List<User>> SearchAsync(string keyword, int page, int pageSize)
        {
            return await _context.Users
                .Where(u =>
                    u.Email.Contains(keyword) ||
                    u.FullName.Contains(keyword) ||
                    u.Id.ToString().Contains(keyword)
                )
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountSearchAsync(string keyword)
        {
            return await _context.Users
                .Where(u =>
                    u.Email.Contains(keyword) ||
                    u.FullName.Contains(keyword) ||
                    u.Id.ToString().Contains(keyword)
                )
                .CountAsync();
        }
        public async Task BlockUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            user.IsBlocked = true;

            await _context.SaveChangesAsync();
        }
        public async Task UnblockUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            user.IsBlocked = false;

            await _context.SaveChangesAsync();
        }
        public async Task SoftDeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            user.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
        public async Task RestoreUserAsync(Guid userId)
        {
            var user = await _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            user.IsDeleted = false;

            await _context.SaveChangesAsync();
        }
        public async Task UpdateStatusAsync(Guid userId, string status)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");


            await _context.SaveChangesAsync();
        }

        public async Task AddActivityLogAsync(UserActivityLog log)
        {
            await _context.UserActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserActivityLog>> GetUserActivityAsync(Guid userId)
        {
            return await _context.UserActivityLogs
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<List<UserAddress>> GetUserAddressesAsync(Guid userId)
        {
            return await _context.UserAddresses
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.IsDefault)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<UserAddress?> GetUserAddressByIdAsync(Guid addressId)
        {
            return await _context.UserAddresses
                .FirstOrDefaultAsync(x => x.Id == addressId);
        }

        public async Task AddUserAddressAsync(UserAddress address)
        {
            await _context.UserAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAddressAsync(UserAddress address)
        {
            _context.UserAddresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAddressAsync(UserAddress address)
        {
            _context.UserAddresses.Remove(address);
            await _context.SaveChangesAsync();
        }

        public async Task SetDefaultAddressAsync(Guid userId, Guid addressId)
        {
            var addresses = await _context.UserAddresses
                .Where(x => x.UserId == userId)
                .ToListAsync();

            foreach (var addr in addresses)
            {
                addr.IsDefault = addr.Id == addressId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<UserSession>> GetUserDevicesAsync(Guid userId)
        {
            return await _context.UserSessions
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LoginAt)
                .ToListAsync();
        }

        public async Task<UserSession?> GetUserDeviceByIdAsync(Guid deviceId)
        {
            return await _context.UserSessions
                .FirstOrDefaultAsync(x => x.Id == deviceId);
        }

        public async Task RemoveUserDeviceAsync(UserSession session)
        {
            _context.UserSessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task LogoutAllDevicesAsync(Guid userId)
        {
            var sessions = await _context.UserSessions
                .Where(x => x.UserId == userId && x.IsActive)
                .ToListAsync();

            foreach (var s in sessions)
            {
                s.IsActive = false;
                s.LogoutAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
        public async Task CreateUserDeviceAsync(UserSession session)
        {
            await _context.UserSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _context.Users.IgnoreQueryFilters().CountAsync();
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetBlockedUsersCountAsync()
        {
            return await _context.Users.IgnoreQueryFilters().CountAsync(u => u.IsBlocked);
        }

        public async Task<int> GetDeletedUsersCountAsync()
        {
            return await _context.Users.IgnoreQueryFilters().CountAsync(u => u.IsDeleted);
        }
    }
}

