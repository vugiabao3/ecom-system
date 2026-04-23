using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Users.GetUserAddresses
{
    public class GetUserAddressesQueryHandler
        : IRequestHandler<GetUserAddressesQuery, GetUserAddressesResponse>
    {
        private readonly IUserRepository _repo;

        public GetUserAddressesQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetUserAddressesResponse> Handle(
            GetUserAddressesQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _repo.GetUserAddressesAsync(request.UserId);

            return new GetUserAddressesResponse
            {
                Addresses = data
            };
        }
    }
}