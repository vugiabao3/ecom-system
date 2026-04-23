using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UserService.Application.Users.GetUserRoles
{
    public class GetUserRolesResponse
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = null!;
    }
}