using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.Internal
{
    public class GetUserExistsQuery : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}
