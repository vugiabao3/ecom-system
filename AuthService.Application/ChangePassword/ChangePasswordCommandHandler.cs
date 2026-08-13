using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application.Interfaces;
using EcomSystem.Contracts.Users;


namespace AuthService.Application.ChangePassword
{
    public class ChangePasswordCommandHandler
        : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
    {
        private readonly IAuthUserRepository _authUserRepository;
        private readonly IUserServiceClient _userService; // 🔥 thêm
        private readonly IPasswordHasher _passwordHasher;

        public ChangePasswordCommandHandler(
            IAuthUserRepository authUserRepository,
            IUserServiceClient userService,
            IPasswordHasher passwordHasher)
        {
            _authUserRepository = authUserRepository;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<ChangePasswordResponse> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            // 1. lấy user từ Auth DB
            var user = await _authUserRepository.GetByEmailAsync(request.Email);

            if (user == null)
                throw new Exception("User not found");

            // 2. check trạng thái
            

            if (user.Status != "Active")
                throw new Exception("User is not active");

            // 3. verify password cũ
            var isValid = _passwordHasher.Verify(
                request.OldPassword,
                user.PasswordHash);

            if (!isValid)
                throw new Exception("Old password incorrect");

            // 4. hash password mới
            var newHashed = _passwordHasher.Hash(request.NewPassword);

            // 🔥 5. UPDATE AUTH DB
            await _authUserRepository.UpdatePasswordAsync(user.Id, newHashed);
            await _authUserRepository.SaveChangesAsync(); // 🔥 BẮT BUỘC

            //// 🔥 6. UPDATE USER SERVICE (CỰC QUAN TRỌNG)
            //await _userService.UpdatePasswordAsync(user.Id, newHashed);

            // 🔥 7. LOGOUT ALL DEVICES (đúng chỗ)
            await _userService.LogoutAllDevicesAsync(user.Id);

            return new ChangePasswordResponse
            {
                Success = true,
                Message = "Password changed successfully"
            };
        }
    }
}
