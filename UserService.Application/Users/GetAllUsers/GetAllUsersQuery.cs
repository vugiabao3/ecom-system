using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
namespace UserService.Application.Users.GetAllUsers
{

    public class GetAllUsersQuery : IRequest<GetAllUsersResponse>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
