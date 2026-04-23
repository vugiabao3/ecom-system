using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomSystem.Contracts.Users
{
    public class UpdateUserRequest
    {
        public string? FullName { get; set; }
        public string? PasswordHash { get; set; }
    }
}

