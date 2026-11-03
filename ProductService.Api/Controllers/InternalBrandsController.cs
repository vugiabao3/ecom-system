using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/internal/brands")]
    [Authorize]
    public class InternalBrandsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public InternalBrandsController(AppDbContext db) { _db = db; }

        [HttpGet("{id}/exists")]
        public async Task<IActionResult> Exists(Guid id)
        {
            var exists = await _db.Brands.AnyAsync(b => b.Id == id);
            return Ok(new { exists });
        }
    }
}