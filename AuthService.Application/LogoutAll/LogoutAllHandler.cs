using AuthService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.LogoutAll
{
    public class LogoutAllHandler : IRequestHandler<LogoutAllCommand, LogoutAllResponse>
    {
        private readonly IAuthUserRepository _authUserRepository;

        public LogoutAllHandler(IAuthUserRepository authUserRepository)
        {
            _authUserRepository = authUserRepository;
        }

        public async Task<LogoutAllResponse> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
        {
            var user = await _authUserRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return new LogoutAllResponse { Success = true, Message = "Logged out from all devices" };

            await _authUserRepository.LogoutAllDevicesAsync(user.Id);
            await _authUserRepository.SaveChangesAsync();

            return new LogoutAllResponse { Success = true, Message = "Logged out from all devices" };
        }
    }
}
