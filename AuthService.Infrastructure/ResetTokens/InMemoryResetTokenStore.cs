using AuthService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.ResetTokens
{
    public class InMemoryResetTokenStore : IResetTokenStore
    {
        private static Dictionary<string, (string Email, DateTime ExpireAt)> _store = new();

        public Task SaveAsync(string token, string email)
        {
            _store[token] = (email, DateTime.UtcNow.AddMinutes(10));
            return Task.CompletedTask;
        }

        public Task<string?> GetEmailAsync(string token)
        {
            if (_store.ContainsKey(token))
            {
                var data = _store[token];

                if (data.ExpireAt > DateTime.UtcNow)
                    return Task.FromResult<string?>(data.Email);
            }

            return Task.FromResult<string?>(null);
        }

        public Task DeleteAsync(string token)
        {
            _store.Remove(token);
            return Task.CompletedTask;
        }
    }
}
