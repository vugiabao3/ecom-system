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

namespace UserService.Application.Users.GetAllUsers
{
   

    public class GetAllUsersQueryHandler
        : IRequestHandler<GetAllUsersQuery, GetAllUsersResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetAllUsersResponse> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(
                request.Page, request.PageSize);

            var total = await _userRepository.CountAsync();
            await _userRepository.AddActivityLogAsync(new UserActivityLog
            {
                
                Action = ActivityActions.GetAllUsers,
                Description = "User was Getalluser"
            });
            return new GetAllUsersResponse
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
