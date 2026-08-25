using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EcomSystem.Contracts.Enums;

namespace UserService.Application.Users.UpdateUser
{
    public class UpdateUserResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public UserRole Role { get; set; }
        public string? CurrentAddress { get; set; }
        public string? CurrentLocation { get; set; }
    }
}
