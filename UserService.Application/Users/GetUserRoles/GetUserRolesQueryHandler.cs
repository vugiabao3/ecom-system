using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace UserService.Application.Users.GetUserRoles
{
    public class GetUserRolesQueryHandler
        : IRequestHandler<GetUserRolesQuery, GetUserRolesResponse>
    {
        private readonly IUserRepository _repo;

        public GetUserRolesQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetUserRolesResponse> Handle(
            GetUserRolesQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(request.UserId);
            await _repo.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.GetUserRole,
                Description = "User was GetUserRoles"
            });
            if (user == null)
                throw new Exception("User not found");

            return new GetUserRolesResponse
            {
                UserId = user.Id,
                Role = user.Role
            };
        }
    }
}
