using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IRefreshTokenStore
    {
        Task SaveAsync(string refreshToken, string email);
        Task<string?> GetAsync(string refreshToken);
        Task DeleteAsync(string refreshToken);
    }
}
