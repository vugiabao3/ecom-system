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

namespace UserService.Application.Users.GetUserProfile
{
    public class GetUserProfileQueryHandler
        : IRequestHandler<GetUserProfileQuery, GetUserProfileResponse>
    {
        private readonly IUserRepository _repo;

        public GetUserProfileQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetUserProfileResponse> Handle(
            GetUserProfileQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(request.UserId);

            if (user == null)
                return null;

            await _repo.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.GetUserById,
                Description = "User profile was retrieved"
            });

            return new GetUserProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                Avatar = user.Avatar,
                Role = user.Role,
                IsBlocked = user.IsBlocked,
                CreatedAt = user.CreatedAt,
                CurrentAddress = user.CurrentAddress,
                CurrentLocation = user.CurrentLocation
            };
        }
    }
}
