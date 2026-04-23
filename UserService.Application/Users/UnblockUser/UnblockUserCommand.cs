using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using UserService.Application.Users.BlockUser;


namespace UserService.Application.Users.UnblockUser
{

    public class UnblockUserCommand : IRequest<UnblockUserResponse>
    {
        public Guid UserId { get; set; }
    }
}
