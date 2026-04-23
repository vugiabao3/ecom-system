using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Application.Users.RestoreUser
{
    

    public class RestoreUserCommandHandler
        : IRequestHandler<RestoreUserCommand, RestoreUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public RestoreUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RestoreUserResponse> Handle(
            RestoreUserCommand request,
            CancellationToken cancellationToken)
        {
            await _userRepository.RestoreUserAsync(request.UserId);
            await _userRepository.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.RestoreUser,
                Description = "User was RestoreUser"
            });

            return new RestoreUserResponse
            {
                Message = "User restored successfully"
            };
        }
    }
}
