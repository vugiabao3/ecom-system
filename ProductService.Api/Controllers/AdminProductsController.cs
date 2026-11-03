using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Persistence;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AdminProductsController(AppDbContext db) { _db = db; }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> Pending()
        {
            var products = await _db.Products.Where(p => p.Status == "Pending").ToListAsync();
            return Ok(products);
        }

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            product.Status = "Active";
            product.IsApproved = true;
            product.IsActive = true;
            await _db.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] string reason)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            product.Status = "Rejected";
            product.IsApproved = false;
            product.IsActive = false;
            await _db.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPatch("{id}/suspend")]
        public async Task<IActionResult> Suspend(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            product.Status = "Suspended";
            product.IsActive = false;
            await _db.SaveChangesAsync();
            return Ok(product);
        }
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            product.Status = "Active";
            product.IsActive = true;
            await _db.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            product.IsDeleted = true;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> Statistics()
        {
            var total = await _db.Products.CountAsync();
            var active = await _db.Products.CountAsync(p => p.Status == "Active");
            return Ok(new { total, active });
        }

        [HttpGet("reported")]
        public async Task<IActionResult> Reported()
        {
            return Ok(new List<object>());
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var products = await _db.Products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var total = await _db.Products.CountAsync();
            return Ok(new { items = products, total, page, pageSize });
        }
    }
}