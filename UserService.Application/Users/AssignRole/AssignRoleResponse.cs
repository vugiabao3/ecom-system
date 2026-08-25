using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EcomSystem.Contracts.Enums;

namespace UserService.Application.Users.AssignRole
{
    public class AssignRoleResponse
    {
        public Guid UserId { get; set; }
        public UserRole Role { get; set; }
    }
}
