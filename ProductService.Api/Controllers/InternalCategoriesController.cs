using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/internal/categories")]
    [Authorize]
    public class InternalCategoriesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public InternalCategoriesController(AppDbContext db) { _db = db; }

        [HttpGet("{id}/exists")]
        public async Task<IActionResult> Exists(int id)
        {
            var exists = await _db.Categories.AnyAsync(c => c.Id == id);
            return Ok(new { exists });
        }
    }
}