using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BrandsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _db.Brands.ToListAsync();
            return Ok(brands);
        }

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProducts(Guid id)
        {
            var products = await _db.Products.Where(p => p.BrandId == id && !p.IsDeleted).ToListAsync();
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var brands = await _db.Brands.Where(b => b.Name.Contains(keyword)).ToListAsync();
            return Ok(brands);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PatchStatus(Guid id, [FromBody] bool isActive)
        {
            var brand = await _db.Brands.FindAsync(id);
            if (brand == null) return NotFound();
            brand.IsActive = isActive;
            await _db.SaveChangesAsync();
            return Ok(brand);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var brand = await _db.Brands.FindAsync(id);
            if (brand == null) return NotFound();
            return Ok(brand);
        }
    }
}