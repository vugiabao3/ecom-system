using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CartService.Application.Interfaces;

namespace CartService.Infrastructure.Services
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
            ?? _http.HttpContext?.User?.FindFirst("sub")?.Value
            ?? "";

        public string Role =>
            _http.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value
            ?? "";
    }
}