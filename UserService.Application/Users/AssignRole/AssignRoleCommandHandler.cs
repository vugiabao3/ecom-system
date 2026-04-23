using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Application.Users.AssignRole
{
    public class AssignRoleCommandHandler
        : IRequestHandler<AssignRoleCommand, AssignRoleResponse>
    {
        private readonly IUserRepository _repo;

        public AssignRoleCommandHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<AssignRoleResponse> Handle(
            AssignRoleCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(request.UserId);

            if (user == null)
                throw new Exception("User not found");

            // 🔥 change role

            await _repo.UpdateAsync(user);
            await _repo.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.Assign,
                Description = "User was  assign"
            });

            return new AssignRoleResponse
            {
                UserId = user.Id,
            };
        }
    }
}