using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcomSystem.Contracts.Enums;

namespace UserService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }

        public string? Avatar { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public bool IsBlocked { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public ICollection<UserAddress> Addresses { get; set; }

        public string? CurrentAddress { get; set; }
        public string? CurrentLocation { get; set; }
    }
}
