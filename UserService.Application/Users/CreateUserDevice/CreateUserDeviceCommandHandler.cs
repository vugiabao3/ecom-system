using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Users.CreateUserDevice
{
    public class CreateUserDeviceCommandHandler
        : IRequestHandler<CreateUserDeviceCommand, CreateUserDeviceResponse>
    {
        private readonly IUserRepository _repo;

        public CreateUserDeviceCommandHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<CreateUserDeviceResponse> Handle(
            CreateUserDeviceCommand request,
            CancellationToken cancellationToken)
        {
            var session = new UserSession
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                DeviceInfo = request.DeviceInfo,
                IpAddress = request.IpAddress,
                LoginAt = DateTime.UtcNow,
                IsActive = true
            };

            await _repo.CreateUserDeviceAsync(session);

            return new CreateUserDeviceResponse
            {
                Message = "Device session created"
            };
        }
    }
}