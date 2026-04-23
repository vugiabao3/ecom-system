using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace UserService.Application.Users.RestoreUser
{

    public class RestoreUserCommand : IRequest<RestoreUserResponse>
    {
        public Guid UserId { get; set; }
    }
}
