using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomSystem.Contracts.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; } = "User";
        public string? CurrentAddress { get; set; }
        public string? CurrentLocation { get; set; }
    }
}
