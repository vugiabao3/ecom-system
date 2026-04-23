using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Entities
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string DeviceInfo { get; set; } = string.Empty; // Chrome, iPhone...
        public string IpAddress { get; set; } = string.Empty;

        public DateTime LoginAt { get; set; }
        public DateTime? LogoutAt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}