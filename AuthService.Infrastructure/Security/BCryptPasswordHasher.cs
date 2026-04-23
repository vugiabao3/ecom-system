using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using AuthService.Application.Interfaces;

namespace AuthService.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        // 1. Hash password
        public string Hash(string password)
        {
            // Tạo salt + hash password
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // 2. Verify password
        public bool Verify(string password, string hashedPassword)
        {
            // So sánh password nhập vào với password đã hash
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
