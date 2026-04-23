using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Application.DTOs;
namespace AuthService.Application.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<GoogleUserInfo> ValidateAsync(string idToken);
    }
}
