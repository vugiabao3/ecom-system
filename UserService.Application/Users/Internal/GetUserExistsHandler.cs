using MediatR;
using UserService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.Internal
{
    public class GetUserExistsHandler : IRequestHandler<GetUserExistsQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public GetUserExistsHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(GetUserExistsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            return user != null;
        }
    }
}
