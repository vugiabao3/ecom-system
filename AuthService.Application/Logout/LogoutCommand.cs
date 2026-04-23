using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace AuthService.Application.Logout
{

    namespace AuthService.Application.Logout
    {
        public class LogoutCommand : IRequest<LogoutResponse>
        {
            public string RefreshToken { get; set; }
        }
    }
}
