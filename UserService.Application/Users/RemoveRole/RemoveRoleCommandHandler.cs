using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Application.Users.RemoveRole;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

public class RemoveRoleCommandHandler
    : IRequestHandler<RemoveRoleCommand, RemoveRoleResponse>
{
    private readonly IUserRepository _userRepository;

    public RemoveRoleCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RemoveRoleResponse> Handle(
        RemoveRoleCommand request,
        CancellationToken cancellationToken)
    {
        await _userRepository.AddActivityLogAsync(new UserActivityLog
        {
            UserId = request.UserId,
            Action = ActivityActions.RemoveRole,
            Description = "User was RemoveRole"
        });
        return new RemoveRoleResponse
        {
            Message = "Role removed successfully"
        };
    }
}