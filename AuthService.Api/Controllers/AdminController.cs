using AuthService.Application.Interfaces;
using EcomSystem.Contracts.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    
    public class AdminController : ControllerBase
    {
        private readonly IAuthUserRepository _repo;

        public AdminController(IAuthUserRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("set-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetRole(Guid userId, UserRole role)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            user.Role = role;

            await _repo.SaveChangesAsync(); // ✅ sửa ở đây

            return Ok("Role updated");
        }

        [HttpPost("set-active")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetActive(Guid userId, string Status)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            user.Status = Status;

            await _repo.SaveChangesAsync(); // ✅ sửa ở đây

            return Ok("Status updated");
        }
    }
}