using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.GetMe
{
    public class GetMeQuery : IRequest<GetMeResponse>
    {
        public Guid UserId { get; set; }
    }
}
