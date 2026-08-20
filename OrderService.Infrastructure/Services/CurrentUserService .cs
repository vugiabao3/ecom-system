using OrderService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace OrderService.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _http;

        public CurrentUserService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public string UserId =>
            _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? _http.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? _http.HttpContext?.User?.FindFirst("sub")?.Value;

        public string Role =>
            _http.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
    }
}
