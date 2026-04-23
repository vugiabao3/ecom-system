using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthService.Application.Interfaces;
namespace AuthService.Application.Interfaces
{
    public interface IResetTokenStore
    {
        Task SaveAsync(string token, string email);
        Task<string?> GetEmailAsync(string token);
        Task DeleteAsync(string token);
    }
}
