using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace UserService.Application.Users.SoftDeleteUser
{

    public class SoftDeleteUserCommand : IRequest<SoftDeleteUserResponse>
    {
        public Guid UserId { get; set; }
    }
}
