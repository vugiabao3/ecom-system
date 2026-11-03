using MediatR;
using UserService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.GetUserStatistics
{
    public class GetUserStatisticsHandler : IRequestHandler<GetUserStatisticsQuery, GetUserStatisticsResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserStatisticsHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserStatisticsResponse> Handle(GetUserStatisticsQuery request, CancellationToken cancellationToken)
        {
            var total = await _userRepository.GetTotalUsersCountAsync();
            var active = await _userRepository.GetActiveUsersCountAsync();
            var blocked = await _userRepository.GetBlockedUsersCountAsync();
            var deleted = await _userRepository.GetDeletedUsersCountAsync();

            return new GetUserStatisticsResponse
            {
                TotalUsers = total,
                ActiveUsers = active,
                BlockedUsers = blocked,
                DeletedUsers = deleted
            };
        }
    }
}
