using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Application.Users.UpdateUser
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly IUserRepository _repo;

        public UpdateUserCommandHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<UpdateUserResponse> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(request.Id);

            if (user == null)
                throw new Exception("User not found");

            // 🔥 update fields
            user.FullName = request.FullName;
            user.Phone = request.Phone;
            user.Avatar = request.Avatar;

            await _repo.UpdateAsync(user);
            await _repo.AddActivityLogAsync(new UserActivityLog
            {
                
                Action = ActivityActions.UpdateUser,
                Description = "User was UpdateUser"
            });
            return new UpdateUserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Phone = user.Phone,
                Avatar = user.Avatar
            };
        }
    }
}
