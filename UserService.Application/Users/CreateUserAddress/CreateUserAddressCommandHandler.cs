using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Application.Users.CreateUserAddress.UserService.Application.Users.CreateUserAddress;
using UserService.Domain.Entities;

namespace UserService.Application.Users.CreateUserAddress
{
    public class CreateUserAddressCommandHandler
        : IRequestHandler<CreateUserAddressCommand, CreateUserAddressResponse>
    {
        private readonly IUserRepository _repo;

        public CreateUserAddressCommandHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<CreateUserAddressResponse> Handle(
            CreateUserAddressCommand request,
            CancellationToken cancellationToken)
        {
            var address = new UserAddress
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                FullName = request.FullName,
                Phone = request.Phone,
                AddressLine = request.AddressLine,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode
            };

            await _repo.AddUserAddressAsync(address);

            return new CreateUserAddressResponse
            {
                Message = "Address created successfully"
            };
        }
    }
}