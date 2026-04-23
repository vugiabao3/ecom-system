using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace UserService.Application.Users.CreateUser
{
    public class CreateUserCommand : IRequest<CreateUserResponse>
    {
        public Guid Id { get; set; } // 🔥 bắt buộc
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Password { get; set; }
    }
}
