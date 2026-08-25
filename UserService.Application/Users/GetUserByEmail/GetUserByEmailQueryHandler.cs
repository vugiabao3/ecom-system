using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Application.Users.GetUserByEmail
{
    public class GetUserByEmailQueryHandler
        : IRequestHandler<GetUserByEmailQuery, GetUserByEmailResponse>
    {
        private readonly IUserRepository _repo;

        public GetUserByEmailQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetUserByEmailResponse> Handle(
            GetUserByEmailQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByEmailAsync(request.Email);
            await _repo.AddActivityLogAsync(new UserActivityLog
            {
                
                Action = ActivityActions.GetUserByEmail,
                Description = "User was GetUserByEmail"
            });
            if (user == null)
                return null;

            return new GetUserByEmailResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
        }
    }
}