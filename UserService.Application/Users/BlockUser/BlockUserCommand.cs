using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace UserService.Application.Users.BlockUser
{

    public class BlockUserCommand : IRequest<BlockUserResponse>
    {
        public Guid UserId { get; set; }
    }
}
