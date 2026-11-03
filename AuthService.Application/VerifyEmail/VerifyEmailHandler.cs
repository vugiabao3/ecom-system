using AuthService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.VerifyEmail
{
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, VerifyEmailResponse>
    {
        private readonly IResetTokenStore _resetTokenStore;
        private readonly IAuthUserRepository _authUserRepository;

        public VerifyEmailHandler(IResetTokenStore resetTokenStore, IAuthUserRepository authUserRepository)
        {
            _resetTokenStore = resetTokenStore;
            _authUserRepository = authUserRepository;
        }

        public async Task<VerifyEmailResponse> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var email = await _resetTokenStore.GetEmailAsync(request.Token);
            if (string.IsNullOrEmpty(email))
                return new VerifyEmailResponse { Success = false, Message = "Invalid or expired token" };

            var user = await _authUserRepository.GetByEmailAsync(email);
            if (user == null)
                return new VerifyEmailResponse { Success = false, Message = "User not found" };

            user.Status = "Active";
            await _authUserRepository.SaveChangesAsync();
            await _resetTokenStore.DeleteAsync(request.Token);

            return new VerifyEmailResponse { Success = true, Message = "Email verified successfully" };
        }
    }
}
