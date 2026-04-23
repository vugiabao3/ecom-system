using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace UserService.Application.Users.LogoutAllDevices
{
    public class LogoutAllDevicesCommand : IRequest<LogoutAllDevicesResponse>
    {
        public Guid UserId { get; set; }
    }
}
