using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/internal/products")]
    [Authorize]
    public class InternalProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public InternalProductsController(AppDbContext db) { _db = db; }

        [HttpPost("batch")]
        public async Task<IActionResult> Batch([FromBody] List<Guid> ids)
        {
            var products = await _db.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}/exists")]
        public async Task<IActionResult> Exists(Guid id)
        {
            var exists = await _db.Products.AnyAsync(p => p.Id == id);
            return Ok(new { exists });
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> Status(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            return Ok(new { product.Id, product.Status, product.IsActive });
        }
        public async Task<IActionResult> Summary(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            return Ok(new { product.Id, product.Name, product.Price, product.IsActive, product.Status });
        }
    }
}