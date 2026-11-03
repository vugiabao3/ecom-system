using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.ResendVerification
{
    public class ResendVerificationCommand : IRequest<ResendVerificationResponse>
    {
        public string Email { get; set; }
    }
}
