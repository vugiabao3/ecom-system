using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<VerifyEmailResponse>
    {
        public string Token { get; set; }
    }
}
