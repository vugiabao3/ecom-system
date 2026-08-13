using AuthService.Application.ForgotPassword;
using AuthService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ForgotPasswordCommandHandler
    : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponse>
{
    private readonly IUserServiceClient _userService;
    private readonly IResetTokenStore _resetTokenStore;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(
        IUserServiceClient userService,
        IResetTokenStore resetTokenStore,
        IEmailService emailService)
    {
        _userService = userService;
        _resetTokenStore = resetTokenStore;
        _emailService = emailService;
    }

    public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        // 1. check user tồn tại
        var user = await _userService.GetUserByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("User not found");

        // 2. tạo reset token
        var resetToken = new Random()
    .Next(100000, 999999)
    .ToString();

        // 3. lưu vào Redis (TTL 5-15 phút)
        await _resetTokenStore.SaveAsync(resetToken, request.Email);

        // 4. gửi email qua Notification Service
        await _emailService.SendResetPasswordEmailAsync(request.Email, resetToken);

        return new ForgotPasswordResponse
        {
            Success = true,
            Message = "Reset password link sent to email"
        };
    }
}
