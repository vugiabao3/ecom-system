using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PaymentService.Infrastructure.Security
{
    public class InternalTokenGenerator
    {
        private readonly IConfiguration _config;

        public InternalTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string Generate()
        {
            // 🔥 lấy section
            var jwt = _config.GetSection("InternalJwt");

            var keyString = jwt["Key"];
            var issuer = jwt["Issuer"];
            var audience = jwt["Audience"];

            // 🚨 tránh lỗi null như bạn vừa bị
            if (string.IsNullOrEmpty(keyString))
                throw new Exception("InternalJwt:Key is missing in appsettings.json");

            if (string.IsNullOrEmpty(issuer))
                throw new Exception("InternalJwt:Issuer is missing");

            if (string.IsNullOrEmpty(audience))
                throw new Exception("InternalJwt:Audience is missing");

            var key = Encoding.UTF8.GetBytes(keyString);

            var claims = new[]
            {
                new Claim("service", "PaymentService")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}