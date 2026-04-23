using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Users.LogoutAllDevices
{
    public class LogoutAllDevicesCommandHandler
        : IRequestHandler<LogoutAllDevicesCommand, LogoutAllDevicesResponse>
    {
        private readonly IUserRepository _repo;

        public LogoutAllDevicesCommandHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<LogoutAllDevicesResponse> Handle(
            LogoutAllDevicesCommand request,
            CancellationToken cancellationToken)
        {
            await _repo.LogoutAllDevicesAsync(request.UserId);

            return new LogoutAllDevicesResponse
            {
                Message = "Logged out all devices successfully"
            };
        }
    }
}
