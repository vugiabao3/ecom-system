using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Application.Users.BlockUser
{


    public class BlockUserCommandHandler
        : IRequestHandler<BlockUserCommand, BlockUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public BlockUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BlockUserResponse> Handle(
            BlockUserCommand request,
            CancellationToken cancellationToken)
        {
            await _userRepository.BlockUserAsync(request.UserId);
            await _userRepository.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.Block,
                Description = "User was blocked"
            });
            return new BlockUserResponse
            {
                Message = "User blocked successfully"
            };

            
        }

    }
}
