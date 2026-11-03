using MediatR;
using UserService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.Internal
{
    public class GetUserSummaryHandler : IRequestHandler<GetUserSummaryQuery, GetUserSummaryResponse?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserSummaryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserSummaryResponse?> Handle(GetUserSummaryQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null) return null;

            return new GetUserSummaryResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
        }
    }
}
