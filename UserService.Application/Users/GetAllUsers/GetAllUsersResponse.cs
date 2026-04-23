using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;
namespace UserService.Application.Users.GetAllUsers
{
    public class GetAllUsersResponse
    {
        public List<UserDto> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
