using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.GetMe
{
    public class GetMeResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public string? CurrentAddress { get; set; }
        public string? CurrentLocation { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
