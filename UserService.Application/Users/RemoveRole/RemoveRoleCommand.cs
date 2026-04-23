using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Users.RemoveRole;

using MediatR;

public class RemoveRoleCommand : IRequest<RemoveRoleResponse>
{
    public Guid UserId { get; set; }
    public string Role { get; set; }
}