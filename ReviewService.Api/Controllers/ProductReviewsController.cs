using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Persistence;

namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/products/{productId}/reviews")]
public class ProductReviewsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductReviewsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public async Task<IActionResult> List(Guid productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var items = await _db.ProductReviews
            .Where(r => r.ProductId == productId && r.Status == ReviewStatus.Published)
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
        return Ok(items);
    }
    [HttpGet("summary")]
    public async Task<IActionResult> Summary(Guid productId)
    {
        var reviews = await _db.ProductReviews.Where(r => r.ProductId == productId && r.Status == ReviewStatus.Published).ToListAsync();
        var avg = reviews.Any() ? (decimal)reviews.Average(r => r.Rating) : 0m;
        return Ok(new { productId, average = Math.Round(avg,1), total = reviews.Count });
    }
    [HttpGet("distribution")]
    public async Task<IActionResult> Distribution(Guid productId)
    {
        var groups = await _db.ProductReviews.Where(r => r.ProductId == productId && r.Status == ReviewStatus.Published)
            .GroupBy(r => r.Rating).Select(g => new { rating = g.Key, count = g.Count() }).ToListAsync();
        return Ok(new { productId, groups });
    }
    [HttpGet("recent")]
    public async Task<IActionResult> Recent(Guid productId, [FromQuery] int take = 5)
    {
        var items = await _db.ProductReviews.Where(r => r.ProductId == productId && r.Status == ReviewStatus.Published)
            .OrderByDescending(r => r.CreatedAt).Take(take).ToListAsync();
        return Ok(items);
    }
    [HttpGet("top")]
    public async Task<IActionResult> Top(Guid productId, [FromQuery] int take = 5)
    {
        var items = await _db.ProductReviews.Where(r => r.ProductId == productId && r.Status == ReviewStatus.Published)
            .OrderByDescending(r => r.CreatedAt).Take(take).ToListAsync();
        return Ok(items);
    }
    [HttpGet("with-media")]
    public async Task<IActionResult> WithMedia(Guid productId)
    {
        var ids = await _db.ReviewMedia.Select(m => m.ReviewId).ToListAsync();
        var items = await _db.ProductReviews.Where(r => r.ProductId == productId && r.Status == ReviewStatus.Published && ids.Contains(r.Id)).ToListAsync();
        return Ok(items);
    }
    [HttpGet("filter")]
    public async Task<IActionResult> Filter(Guid productId, [FromQuery] int? rating = null)
    {
        var q = _db.ProductReviews.Where(r => r.ProductId == productId && r.Status == ReviewStatus.Published);
        if (rating.HasValue) q = q.Where(r => r.Rating == rating.Value);
        return Ok(await q.ToListAsync());
    }
}
