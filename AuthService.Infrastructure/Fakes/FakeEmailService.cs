using AuthService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Fakes
{
    public class FakeEmailService : IEmailService
    {
        public Task SendResetPasswordEmailAsync(string email, string token)
        {
            Console.WriteLine($"Send email to {email} with token {token}");
            return Task.CompletedTask;
        }
    }
}
