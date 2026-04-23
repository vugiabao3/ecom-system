using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AuthService.Application.Interfaces;
using AuthService.Application.DTOs;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using AuthService.Domain.Entities;
namespace AuthService.Infrastructure.Repositories
{

    public class AuthUserRepository : IAuthUserRepository
    {
        private readonly AppDbContext _context;

        public AuthUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuthUser?> GetByEmailAsync(string email)
        {
            return await _context.AuthUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<AuthUser?> GetByIdAsync(Guid id)
        {
            return await _context.AuthUsers
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(AuthUser user)
        {
            await _context.AuthUsers.AddAsync(user);
        }

        public async Task UpdatePasswordAsync(Guid userId, string passwordHash)
        {
            var user = await _context.AuthUsers.FindAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            user.PasswordHash = passwordHash;
        }

        public async Task UpdateStatusAsync(Guid userId, string status)
        {
            var user = await _context.AuthUsers.FindAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            user.Status = status;
        }

        public async Task LogoutAllDevicesAsync(Guid userId)
        {
            var sessions = await _context.UserSessions
                .Where(x => x.UserId == userId && x.IsActive)
                .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsActive = false;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}