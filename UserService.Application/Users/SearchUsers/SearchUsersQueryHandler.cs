using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;
namespace UserService.Application.Users.SearchUsers
{
   

    public class SearchUsersQueryHandler
        : IRequestHandler<SearchUsersQuery, SearchUsersResponse>
    {
        private readonly IUserRepository _userRepository;

        public SearchUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<SearchUsersResponse> Handle(
            SearchUsersQuery request,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.SearchAsync(
                request.Keyword,
                request.Page,
                request.PageSize);

            var total = await _userRepository.CountSearchAsync(request.Keyword);
            await _userRepository.AddActivityLogAsync(new UserActivityLog
            {
                
                Action = ActivityActions.SearchUsers,
                Description = "User was SearchUsers"
            });
            return new SearchUsersResponse
            {
                Users = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                }).ToList(),

                TotalCount = total
            };
        }
    }
}
