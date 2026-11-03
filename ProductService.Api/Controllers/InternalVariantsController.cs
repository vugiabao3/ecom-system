using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/internal/variants")]
    [Authorize]
    public class InternalVariantsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public InternalVariantsController(AppDbContext db) { _db = db; }

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> Summary(Guid id)
        {
            var variant = await _db.ProductVariants.FindAsync(id);
            if (variant == null) return NotFound();
            return Ok(new { variant.Id, variant.ProductId, variant.SKU, variant.Price, variant.IsActive });
        }

        [HttpPost("batch")]
        public async Task<IActionResult> Batch([FromBody] List<Guid> ids)
        {
            var variants = await _db.ProductVariants.Where(v => ids.Contains(v.Id)).ToListAsync();
            return Ok(variants);
        }
    }
}