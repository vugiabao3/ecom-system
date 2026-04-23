using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace UserService.Application.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest<UpdateUserResponse>
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string PasswordHash { get; set; }
    }
}
