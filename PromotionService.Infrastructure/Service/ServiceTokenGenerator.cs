//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.IdentityModel.Tokens;

//using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;

//namespace PromotionService.Infrastructure.Service
//{

//    public class ServiceTokenGenerator
//    {
//        private readonly IConfiguration _config;

//        public ServiceTokenGenerator(IConfiguration config)
//        {
//            _config = config;
//        }

//        public string Generate()
//        {
//            var jwt = _config.GetSection("JwtInternal");

//            var key = Encoding.UTF8.GetBytes(jwt["Key"]);

//            var token = new JwtSecurityToken(
//                issuer: jwt["Issuer"],
//                audience: jwt["Audience"],
//                claims: new[]
//                {
//                new Claim("service", "OrderService")
//                },
//                expires: DateTime.UtcNow.AddMinutes(30),
//                signingCredentials: new SigningCredentials(
//                    new SymmetricSecurityKey(key),
//                    SecurityAlgorithms.HmacSha256)
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
