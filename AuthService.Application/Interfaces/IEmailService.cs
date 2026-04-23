using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendResetPasswordEmailAsync(string email, string token);
    }
}
