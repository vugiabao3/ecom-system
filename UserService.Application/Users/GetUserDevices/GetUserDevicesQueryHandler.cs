using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Users.GetUserDevices
{
    public class GetUserDevicesQueryHandler
        : IRequestHandler<GetUserDevicesQuery, GetUserDevicesResponse>
    {
        private readonly IUserRepository _repo;

        public GetUserDevicesQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetUserDevicesResponse> Handle(
            GetUserDevicesQuery request,
            CancellationToken cancellationToken)
        {
            var sessions = await _repo.GetUserDevicesAsync(request.UserId);

            return new GetUserDevicesResponse
            {
                Devices = sessions.Select(x => new DeviceItem
                {
                    DeviceInfo = x.DeviceInfo,
                    IpAddress = x.IpAddress,
                    LoginAt = x.LoginAt
                }).ToList()
            };
        }
    }
}