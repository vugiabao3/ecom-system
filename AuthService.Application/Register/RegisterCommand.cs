using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace AuthService.Application.Register
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        // Email người dùng gửi lên
        public string Email { get; set; }

        // Password người dùng gửi lên
        public string Password { get; set; }
    }
}
