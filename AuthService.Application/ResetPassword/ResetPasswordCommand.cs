using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application.ResetPassword;
namespace AuthService.Application.ResetPassword
{ 
        public class ResetPasswordCommand : IRequest<ResetPasswordResponse>
        {
            public string Token { get; set; }
            public string NewPassword { get; set; }
        }
    }

