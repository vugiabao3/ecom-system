using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;

namespace AuthService.Infrastructure.ExternalServices
{
    public class GoogleAuthService : IGoogleAuthService
    {
        public async Task<GoogleUserInfo> ValidateAsync(string idToken)
        {
            // giả lập (DEV MODE)
            // production sẽ gọi Google API verify token

            return new GoogleUserInfo
            {
                Email = "test@gmail.com",
                Name = "Google User"
            };
        }
    }
}