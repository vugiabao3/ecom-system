using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Application.Interfaces;


namespace AuthService.Infrastructure.RefreshTokens
{
    

    public class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private static Dictionary<string, string> _store = new();

        public Task SaveAsync(string refreshToken, string email)
        {
            _store[refreshToken] = email;
            return Task.CompletedTask;
        }

        public Task<string?> GetAsync(string refreshToken)
        {
            _store.TryGetValue(refreshToken, out var email);
            return Task.FromResult(email);
        }

        public Task DeleteAsync(string refreshToken)
        {
            _store.Remove(refreshToken);
            return Task.CompletedTask;
        }
    } 
}
