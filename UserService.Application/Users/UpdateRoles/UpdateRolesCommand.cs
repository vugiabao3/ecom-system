using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace UserService.Application.Users.UpdateRoles
{

    public class UpdateRolesCommand : IRequest<UpdateRolesResponse>
    {
        public Guid UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
