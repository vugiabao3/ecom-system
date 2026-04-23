using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace UserService.Application.Users.UpdateUserStatus
{
  
    public class UpdateUserStatusCommand : IRequest<UpdateUserStatusResponse>
    {
        public Guid UserId { get; set; }
        public string Status { get; set; }
    }
}
