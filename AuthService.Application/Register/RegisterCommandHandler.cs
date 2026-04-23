using AuthService.Application.Common.Exceptions;
using EcomSystem.Contracts.Users;
using AuthService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AuthService.Domain.Entities; // 🔥 thêm



namespace AuthService.Application.Register
{
    public class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserServiceClient _userServiceClient;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthUserRepository _authUserRepository;

        public RegisterCommandHandler(
            IUserServiceClient userServiceClient,
            IPasswordHasher passwordHasher,
            IAuthUserRepository authUserRepository)
        {
            _userServiceClient = userServiceClient;
            _passwordHasher = passwordHasher;
            _authUserRepository = authUserRepository;
        }

        public async Task<RegisterResponse> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            // 🔥 1. check tồn tại (check cả 2 DB cho chắc)
            var existingUser = await _userServiceClient.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                throw new BadRequestException("Email already exists");

            var existingAuthUser = await _authUserRepository.GetByEmailAsync(request.Email);
            if (existingAuthUser != null)
                throw new BadRequestException("Email already exists in auth");

            // 🔥 2. hash password 1 lần duy nhất
            var hashedPassword = _passwordHasher.Hash(request.Password);
            // 🔥 3. tạo ID trước
            var userId = Guid.NewGuid();
            // 🔥 3. tạo user bên UserService
            await _userServiceClient.CreateUserAsync(new CreateUserRequest
            {
                Id = userId, // 🔥 phải có
                Email = request.Email,
                Password = request.Password, // gửi raw cho UserService tự hash
                FullName = "Default User"
            });

            // 🔥 4. tạo user bên Auth DB (dùng CÙNG ID)
            var authUser = new AuthUser
            {
                Id = userId, // 🔥 quan trọng nhất
                Email = request.Email,
                PasswordHash = hashedPassword, // dùng hash đã tạo
                Role = "User",
                Status = "Active",
            };

            await _authUserRepository.CreateAsync(authUser);
            await _authUserRepository.SaveChangesAsync();

            // 5. response
            return new RegisterResponse
            {
                Success = true,
                Message = "Register successfully"
            };
        }
    }
}
