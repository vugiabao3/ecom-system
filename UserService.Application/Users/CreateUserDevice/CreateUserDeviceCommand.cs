using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace UserService.Application.Users.CreateUserDevice
{
    public class CreateUserDeviceCommand : IRequest<CreateUserDeviceResponse>
    {
        public Guid UserId { get; set; }

        public string DeviceInfo { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
    }
}
