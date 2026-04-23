using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;

namespace UserService.Application.Users.SearchUsers
{
    public class SearchUsersResponse
    {
        public List<UserDto> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
