using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;
namespace UserService.Application.Users.SoftDeleteUser
{
 

    public class SoftDeleteUserCommandHandler
        : IRequestHandler<SoftDeleteUserCommand, SoftDeleteUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public SoftDeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<SoftDeleteUserResponse> Handle(
            SoftDeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            await _userRepository.SoftDeleteUserAsync(request.UserId);
            await _userRepository.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.SoftDeleteUser,
                Description = "User was SoftDeleteUser"
            });
            return new SoftDeleteUserResponse
            {
                Message = "User soft deleted successfully"
            };
        }
    }
}
