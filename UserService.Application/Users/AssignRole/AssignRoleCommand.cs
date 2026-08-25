using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EcomSystem.Contracts.Enums;
using MediatR;

namespace UserService.Application.Users.AssignRole
{
    public class AssignRoleCommand : IRequest<AssignRoleResponse>
    {
        public Guid UserId { get; set; }

        public UserRole Role { get; set; }
    }
}
