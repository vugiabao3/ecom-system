using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuthService.Application.OAuth
{
    public class GoogleLoginCommand : IRequest<GoogleLoginResponse>
    {
        public string IdToken { get; set; }
    }
}