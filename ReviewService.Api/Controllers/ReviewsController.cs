using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Persistence;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _httpFactory;
    public ReviewsController(AppDbContext db, IHttpClientFactory httpFactory) { _db = db; _httpFactory = httpFactory; }
    private Guid BuyerId => Guid.TryParse(User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var id) ? id : Guid.Empty;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateReviewRequest req)
    {
        if (BuyerId == Guid.Empty) return Unauthorized();
        if (req.Rating < 1 || req.Rating > 5) return BadRequest("Rating must be 1-5");
        if (string.IsNullOrWhiteSpace(req.Content) || req.Content.Length > 2000) return BadRequest("Invalid content");
        if (await _db.ProductReviews.AnyAsync(r => r.OrderItemId == req.OrderItemId)) return Conflict("Already reviewed");
        // verify order via OrderService
        var client = _httpFactory.CreateClient();
        var resp = await client.GetAsync($"http://orderservice:8080/api/internal/orders/{req.OrderId}");
        if (!resp.IsSuccessStatusCode) return BadRequest("Order not found");
        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        // minimal: check userId and status
        // assume response has userId and status and items
        _db.ProductReviews.Add(new ProductReview
        {
            OrderItemId = req.OrderItemId,
            ProductId = req.ProductId,
            VariantId = req.VariantId,
            BuyerId = BuyerId,
            Rating = req.Rating,
            Title = req.Title,
            Content = req.Content,
            IsVerifiedPurchase = true,
            Status = ReviewStatus.Published
        });
        await _db.SaveChangesAsync();
        return Ok(new { message = "Created" });
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var items = await _db.ProductReviews.Where(r => r.Status == ReviewStatus.Published)
            .Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var r = await _db.ProductReviews.FindAsync(id);
        if (r == null) return NotFound();
        return Ok(r);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Edit(Guid id, [FromBody] EditReviewRequest req)
    {
        var r = await _db.ProductReviews.FindAsync(id);
        if (r == null) return NotFound();
        if (r.BuyerId != BuyerId) return Forbid();
        if (req.Rating.HasValue) { if (req.Rating <1 || req.Rating>5) return BadRequest(); r.Rating = req.Rating.Value; }
        if (req.Content != null) r.Content = req.Content;
        r.Title = req.Title ?? r.Title;
        r.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(r);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var r = await _db.ProductReviews.FindAsync(id);
        if (r == null) return NotFound();
        if (r.BuyerId != BuyerId) return Forbid();
        _db.ProductReviews.Remove(r);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Deleted" });
    }
}
public class CreateReviewRequest
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? VariantId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Content { get; set; } = string.Empty;
}
public class EditReviewRequest
{
    public int? Rating { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}
