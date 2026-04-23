using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace UserService.Application.Users.GetUserActivity
{
    public class GetUserActivityQuery : IRequest<List<GetUserActivityResponse>>
    {
        public Guid UserId { get; set; }
    }
}