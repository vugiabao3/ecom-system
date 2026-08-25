using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcomSystem.Contracts.Enums;

namespace AuthService.Domain.Entities
{
    public class AuthUser
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }

        public string Status { get; set; }
    }
}
