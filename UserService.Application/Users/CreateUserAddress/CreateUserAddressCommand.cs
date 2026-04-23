using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.CreateUserAddress
{
    using MediatR;

    namespace UserService.Application.Users.CreateUserAddress
    {
        public class CreateUserAddressCommand : IRequest<CreateUserAddressResponse>
        {
            public Guid UserId { get; set; }

            public string FullName { get; set; }
            public string Phone { get; set; }

            public string AddressLine { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string PostalCode { get; set; }
        }
    }
}
