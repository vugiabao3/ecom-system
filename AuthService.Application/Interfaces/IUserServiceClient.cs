using EcomSystem.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuthService.Application.Interfaces
{
    public interface IUserServiceClient
    {
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto?> GetUserByIdAsync(Guid userId);

        Task<Guid> CreateUserAsync(CreateUserRequest request);
        // cần thêm lại cho auth flow
        Task UpdatePasswordAsync(Guid userId, string newPasswordHash);
        Task LogoutAllDevicesAsync(Guid userId);
    }
}
