using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Infrastructure.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/seller/products")]
    [Authorize]
    public class SellerProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public SellerProductsController(AppDbContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var sellerId)) return Unauthorized();
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerId);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> Statistics()
        {
            if (!Guid.TryParse(_currentUser.UserId, out var sellerId)) return Unauthorized();
            var products = await _db.Products.Where(p => p.SellerId == sellerId).ToListAsync();
            var stats = new
            {
                total = products.Count,
                draft = products.Count(p => p.Status == "Draft"),
                pending = products.Count(p => p.Status == "Pending"),
                rejected = products.Count(p => p.Status == "Rejected"),
                active = products.Count(p => p.Status == "Active")
            };
            return Ok(stats);
        }

        [HttpGet("drafts")]
        public async Task<IActionResult> Drafts()
        {
            if (!Guid.TryParse(_currentUser.UserId, out var sellerId)) return Unauthorized();
            var products = await _db.Products.Where(p => p.SellerId == sellerId && p.Status == "Draft").ToListAsync();
            return Ok(products);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> Pending()
        {
            if (!Guid.TryParse(_currentUser.UserId, out var sellerId)) return Unauthorized();
            var products = await _db.Products.Where(p => p.SellerId == sellerId && p.Status == "Pending").ToListAsync();
            return Ok(products);
        }

        [HttpPatch("{id}/submit")]
        [Authorize]
        public async Task<IActionResult> Submit(Guid id)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var sellerId)) return Unauthorized();
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerId);
            if (product == null) return NotFound();
            product.Status = "Pending";
            await _db.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPatch("{id}/publish")]
        [Authorize]
        public async Task<IActionResult> Publish(Guid id)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var sellerId)) return Unauthorized();
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerId);
            if (product == null) return NotFound();
            if (product.Status != "Approved" && product.Status != "Active") return BadRequest("Not eligible");
            product.Status = "Active";
            product.IsActive = true;
            await _db.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPatch("{id}/unpublish")]
        [Authorize]
        public async Task<IActionResult> Unpublish(Guid id)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var sellerId)) return Unauthorized();
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerId);
            if (product == null) return NotFound();
            product.Status = "Inactive";
            product.IsActive = false;
            await _db.SaveChangesAsync();
            return Ok(product);
        }

        [HttpGet("{id}/performance")]
        [Authorize]
        public async Task<IActionResult> Performance(Guid id)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var sellerId)) return Unauthorized();
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerId);
            if (product == null) return NotFound();
            return Ok(new { productId = id, status = product.Status });
        }
    }
}