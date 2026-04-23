using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;
namespace UserService.Application.Users.UpdateRoles
{

    public class UpdateRolesCommandHandler
        : IRequestHandler<UpdateRolesCommand, UpdateRolesResponse>
    {
        private readonly IUserRepository _userRepository;

        public UpdateRolesCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UpdateRolesResponse> Handle(
            UpdateRolesCommand request,
            CancellationToken cancellationToken)
        {
            await _userRepository.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.UpdateRoles,
                Description = "User was UpdateRoles"
            });
            return new UpdateRolesResponse
            {
                Message = "Roles updated successfully"
            };
        }
    }
}
