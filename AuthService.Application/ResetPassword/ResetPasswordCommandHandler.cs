using AuthService.Application.Interfaces;
using AuthService.Application.ResetPassword;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.ResetPassword
{
    public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    private readonly IResetTokenStore _resetTokenStore;
    private readonly IAuthUserRepository _authUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserServiceClient _userService;

    public ResetPasswordCommandHandler(
        IResetTokenStore resetTokenStore,
        IAuthUserRepository authUserRepository,
        IPasswordHasher passwordHasher,
        IUserServiceClient userService)
    {
        _resetTokenStore = resetTokenStore;
        _authUserRepository = authUserRepository;
        _passwordHasher = passwordHasher;
        _userService = userService;
    }

    public async Task<ResetPasswordResponse> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Verify token
        var email = await _resetTokenStore.GetEmailAsync(request.Token);

        if (email == null)
            throw new Exception("Invalid or expired reset token");

        // 2. INTERNAL AUTH DATA
        var authUser = await _authUserRepository.GetByEmailAsync(email);

        if (authUser == null)
            throw new Exception("User not found");

       
        if (authUser.Status != "Active")
            throw new Exception("User not active");

        // 3. Hash password
        var newHash = _passwordHasher.Hash(request.NewPassword);

        // 4. Update AUTH DB
        await _authUserRepository.UpdatePasswordAsync(authUser.Id, newHash);

        // 5. Sync USER SERVICE (optional)
        await _userService.LogoutAllDevicesAsync(authUser.Id);

        // 6. Delete token
        await _resetTokenStore.DeleteAsync(request.Token);

        return new ResetPasswordResponse
        {
            Success = true,
            Message = "Password reset successfully"
        };
    }
}
}
