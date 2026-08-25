using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EcomSystem.Contracts.Enums;
using MediatR;

namespace UserService.Application.Users.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<GetUserProfileResponse>
    {
        public Guid UserId { get; set; }

        public GetUserProfileQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
