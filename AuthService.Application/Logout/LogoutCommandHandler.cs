using AuthService.Application.Interfaces;
using AuthService.Application.Logout.AuthService.Application.Logout;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutResponse>
    {
        private readonly IRefreshTokenStore _refreshTokenStore;

        public LogoutCommandHandler(IRefreshTokenStore refreshTokenStore)
        {
            _refreshTokenStore = refreshTokenStore;
        }

        public async Task<LogoutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _refreshTokenStore.DeleteAsync(request.RefreshToken);

            return new LogoutResponse
            {
                Success = true,
                Message = "Logout successful"
            };
        }
    }
}
