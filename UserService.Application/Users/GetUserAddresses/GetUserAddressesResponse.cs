using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserService.Domain.Entities;

namespace UserService.Application.Users.GetUserAddresses
{
    public class GetUserAddressesResponse
    {
        public List<UserAddress> Addresses { get; set; }
    }
}
