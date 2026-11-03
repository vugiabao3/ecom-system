using MediatR;
using UserService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.GetMe
{
    public class GetMeHandler : IRequestHandler<GetMeQuery, GetMeResponse?>
    {
        private readonly IUserRepository _userRepository;

        public GetMeHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetMeResponse?> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null) return null;

            return new GetMeResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                Avatar = user.Avatar,
                Role = user.Role.ToString(),
                IsBlocked = user.IsBlocked,
                CurrentAddress = user.CurrentAddress,
                CurrentLocation = user.CurrentLocation,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
