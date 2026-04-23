using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Application.Users.GetUserActivity
{
    public class GetUserActivityQueryHandler
        : IRequestHandler<GetUserActivityQuery, List<GetUserActivityResponse>>
    {
        private readonly IUserRepository _repo;

        public GetUserActivityQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<GetUserActivityResponse>> Handle(
            GetUserActivityQuery request,
            CancellationToken cancellationToken)
        {
            var logs = await _repo.GetUserActivityAsync(request.UserId);
            await _repo.AddActivityLogAsync(new UserActivityLog
            {
                UserId = request.UserId,
                Action = ActivityActions.GetUserActivity,
                Description = "User was GetActivity"
            });
            return logs.Select(x => new GetUserActivityResponse
            {
                Action = x.Action,
                Description = x.Description,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
    }
}