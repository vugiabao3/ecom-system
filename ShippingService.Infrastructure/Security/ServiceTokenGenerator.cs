using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace ShippingService.Infrastructure.Security
{
    public class ServiceTokenGenerator
    {
        private readonly string _secret;

        public ServiceTokenGenerator(string secret)
        {
            _secret = secret;
        }

        public string GenerateToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim("service", "shipping-service")
        };

            var token = new JwtSecurityToken(
                issuer: "InternalService",
                audience: "InternalService",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
