using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Persistence;
namespace NotificationService.Api.Controllers;
[ApiController]
[Route("api/internal")]
public class InternalNotificationsController : ControllerBase
{
    private readonly AppDbContext _db;
    public InternalNotificationsController(AppDbContext db) { _db = db; }
    [HttpPost("notifications")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] Notification n)
    {
        _db.Notifications.Add(n); await _db.SaveChangesAsync();
        return Ok(new { id = n.Id });
    }
    [HttpPost("notifications/batch")]
    [Authorize]
    public async Task<IActionResult> Batch([FromBody] List<Notification> items)
    {
        _db.Notifications.AddRange(items); await _db.SaveChangesAsync();
        return Ok(new { count = items.Count });
    }
    [HttpPost("notifications/broadcast")]
    [Authorize]
    public async Task<IActionResult> Broadcast([FromBody] Notification n)
    {
        _db.Notifications.Add(n); await _db.SaveChangesAsync();
        return Ok(new { id = n.Id });
    }
    [HttpPost("events")]
    [Authorize]
    public async Task<IActionResult> Event([FromBody] EventInbox e)
    {
        if (await _db.EventInboxes.AnyAsync(x => x.EventId == e.EventId)) return Ok(new { duplicate = true });
        _db.EventInboxes.Add(e); await _db.SaveChangesAsync();
        return Ok(new { message = "Event accepted" });
    }
    [HttpPost("events/batch")]
    [Authorize]
    public async Task<IActionResult> EventsBatch([FromBody] List<EventInbox> items)
    {
        var newItems = new List<EventInbox>();
        foreach (var e in items) if (!await _db.EventInboxes.AnyAsync(x => x.EventId == e.EventId)) newItems.Add(e);
        _db.EventInboxes.AddRange(newItems); await _db.SaveChangesAsync();
        return Ok(new { count = newItems.Count });
    }
    [HttpGet("notifications/{id}")]
    [Authorize]
    public async Task<IActionResult> GetNotification(Guid id) => Ok(await _db.Notifications.FindAsync(id));
    [HttpGet("notifications/users/{userId}/summary")]
    [Authorize]
    public async Task<IActionResult> UserSummary(Guid userId)
    {
        var total = await _db.Notifications.CountAsync(n => n.UserId == userId.ToString());
        var unread = await _db.Notifications.CountAsync(n => n.UserId == userId.ToString() && !n.IsRead);
        return Ok(new { userId, total, unread });
    }
    [HttpGet("notifications/statistics")]
    [Authorize]
    public async Task<IActionResult> Statistics() => Ok(new { total = await _db.Notifications.CountAsync() });
    [HttpGet("queue/statistics")]
    [Authorize]
    public IActionResult QueueStatistics() => Ok(new { depth = 0, consumers = 0 });
    [HttpGet("queue/failed")]
    [Authorize]
    public IActionResult QueueFailed() => Ok(new List<object>());
    [HttpPost("queue/{messageId}/retry")]
    [Authorize]
    public IActionResult QueueRetry(Guid messageId) => Ok(new { messageId, message = "Retry queued" });
    [HttpGet("consumers/status")]
    [Authorize]
    public IActionResult ConsumersStatus() => Ok(new { status = "Connected" });
    [HttpGet("statistics")]
    [Authorize]
    public async Task<IActionResult> InternalStatistics() => Ok(new { total = await _db.Notifications.CountAsync() });
}
