using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Application.Users.GetUserById
{
    public class GetUserByIdQueryHandler
        : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        private readonly IUserRepository _repo;

        public GetUserByIdQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetUserByIdResponse> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(request.Id);

            if (user == null)
                return null;
            await _repo.AddActivityLogAsync(new UserActivityLog
            {
                
                Action = ActivityActions.GetUserById,
                Description = "User was GetUserById"
            });
            return new GetUserByIdResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
        }
    }
}