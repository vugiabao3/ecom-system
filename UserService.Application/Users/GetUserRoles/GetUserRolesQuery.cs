using MediatR;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.GetUserRoles
{
    public class GetUserRolesQuery : IRequest<GetUserRolesResponse>
    {
        public Guid UserId { get; set; }

        public GetUserRolesQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}