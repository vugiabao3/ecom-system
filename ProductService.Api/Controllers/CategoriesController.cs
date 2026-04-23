using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTO;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CategoriesController(AppDbContext db)
        {
            _db = db;
        }

        // 🔥 CREATE CATEGORY
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Category name is required");

            var category = new Category
            {
                Name = request.Name
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                category.Id,
                category.Name
            });
        }

        // 🔥 GET ALL (KHÔNG include Products để tránh nặng + loop)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _db.Categories
                .Select(c => new
                {
                    c.Id,
                    c.Name
                })
                .ToListAsync();

            return Ok(categories);
        }

        // 🔥 GET BY ID (có products nếu cần)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _db.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            return Ok(new
            {
                category.Id,
                category.Name,
                Products = category.Products.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.CategoryId
                })
            });
        }

        // 🔥 UPDATE CATEGORY
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateCategoryRequest request)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            category.Name = request.Name;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                category.Id,
                category.Name
            });
        }

        // 🔥 DELETE CATEGORY
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}