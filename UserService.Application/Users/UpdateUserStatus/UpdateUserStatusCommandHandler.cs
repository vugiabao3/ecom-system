using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;
namespace UserService.Application.Users.UpdateUserStatus
{
    

    public class UpdateUserStatusCommandHandler
        : IRequestHandler<UpdateUserStatusCommand, UpdateUserStatusResponse>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserStatusCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UpdateUserStatusResponse> Handle(
            UpdateUserStatusCommand request,
            CancellationToken cancellationToken)
        {
            // validate status
            var validStatuses = new[] { "Active", "Suspended", "Banned" };

            if (!validStatuses.Contains(request.Status))
                throw new Exception("Invalid status");

            await _userRepository.UpdateStatusAsync(request.UserId, request.Status);
            await _userRepository.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.UpdateUserStatus,
                Description = "User was UpdateUserStatus"
            });
            return new UpdateUserStatusResponse
            {
                Message = "User status updated"
            };
        }
    }
}
