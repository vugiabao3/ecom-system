using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using UserService.Application.Users;

namespace UserService.Application.Users.GetUserAddresses
{
    public class GetUserAddressesQuery : IRequest<GetUserAddressesResponse>
    {
        public Guid UserId { get; set; }
    }
}