using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderService.Infrastructure.Security
{
    public class ServiceTokenGenerator
    {
        private readonly IConfiguration _config;

        public ServiceTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string Generate()
        {
            // 🔥 1. Lấy đúng section
            var jwt = _config.GetSection("InternalJwt");

            // 🔥 2. Lấy từng giá trị
            var keyString = jwt["Key"];
            var issuer = jwt["Issuer"];
            var audience = jwt["Audience"];

            // 🔥 3. Validate config (CỰC QUAN TRỌNG)
            if (string.IsNullOrWhiteSpace(keyString))
                throw new Exception("❌ InternalJwt:Key is missing in appsettings.json");

            if (string.IsNullOrWhiteSpace(issuer))
                throw new Exception("❌ InternalJwt:Issuer is missing");

            if (string.IsNullOrWhiteSpace(audience))
                throw new Exception("❌ InternalJwt:Audience is missing");

            // 🔥 4. Convert key
            var key = Encoding.UTF8.GetBytes(keyString);

            // 🔥 5. Tạo token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: new[]
                {
                    new Claim("service", "OrderService")
                },
                expires: DateTime.UtcNow.AddMinutes(5), // 🔥 nên ngắn (internal)
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            );

            // 🔥 6. Return JWT
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}