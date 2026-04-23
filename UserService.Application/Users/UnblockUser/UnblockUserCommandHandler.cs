using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;
namespace UserService.Application.Users.UnblockUser
{

    public class UnblockUserCommandHandler
        : IRequestHandler<UnblockUserCommand, UnblockUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public UnblockUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UnblockUserResponse> Handle(
            UnblockUserCommand request,
            CancellationToken cancellationToken)
        {
            await _userRepository.UnblockUserAsync(request.UserId);
            await _userRepository.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.UnblockUser,
                Description = "User was UnblockUser"
            });
            return new UnblockUserResponse
            {
                Message = "User unblocked successfully"
            };
        }
    }
}
