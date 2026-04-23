using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EcomSystem.Contracts.Users
{
    public class CreateUserRequest
    {
        public Guid Id { get; set; } // 🔥 thêm dòng này
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}
