using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Entities
{
    public class UserActivityLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Action { get; set; } = string.Empty;
        // ví dụ: LOGIN, BLOCK, UPDATE_ROLE

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}