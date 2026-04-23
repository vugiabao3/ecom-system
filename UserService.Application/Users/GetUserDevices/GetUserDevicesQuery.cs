using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace UserService.Application.Users.GetUserDevices
{
    public class GetUserDevicesQuery : IRequest<GetUserDevicesResponse>
    {
        public Guid UserId { get; set; }
    }
}
