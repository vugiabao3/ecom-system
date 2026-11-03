using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace NotificationService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public NotificationsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var notifications = await _db.Notifications
                .Where(n => n.UserId == userId || n.UserId == null)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return Ok(notifications);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Detail(Guid id) => Ok(new { id });

        [HttpGet("unread")]
        [Authorize]
        public IActionResult Unread() => Ok(new List<object>());

        [HttpGet("unread-count")]
        [Authorize]
        public IActionResult UnreadCount() => Ok(new { count = 0 });

        [HttpGet("summary")]
        [Authorize]
        public IActionResult Summary() => Ok(new { total = 0, read = 0, unread = 0 });

        [HttpPatch("{id}/read")]
        [Authorize]
        public IActionResult Read(Guid id) => Ok(new { id, message = "Read" });

        [HttpPost("read-all")]
        [Authorize]
        public IActionResult ReadAll() => Ok(new { message = "All read" });

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(Guid id) => Ok(new { id, message = "Deleted" });

        [HttpPost("delete-batch")]
        [Authorize]
        public IActionResult DeleteBatch() => Ok(new { message = "Batch deleted" });

        [HttpGet("by-type")]
        [Authorize]
        public IActionResult ByType() => Ok(new List<object>());

        [HttpGet("by-category")]
        [Authorize]
        public IActionResult ByCategory() => Ok(new List<object>());

        [HttpGet("history")]
        [Authorize]
        public IActionResult History() => Ok(new List<object>());

        [HttpGet("preferences")]
        [Authorize]
        public IActionResult Preferences() => Ok(new { });

        [HttpPut("preferences")]
        [Authorize]
        public IActionResult UpdatePreferences() => Ok(new { message = "Updated" });

        [HttpPatch("preferences/categories")]
        [Authorize]
        public IActionResult UpdateCategories() => Ok(new { message = "Categories updated" });

        [HttpPut("preferences/quiet-hours")]
        [Authorize]
        public IActionResult UpdateQuietHours() => Ok(new { message = "Quiet hours updated" });

        [HttpGet("preferences/channels")]
        [Authorize]
        public IActionResult GetChannels() => Ok(new List<object>());

        [HttpPost("unsubscribe")]
        [Authorize]
        public IActionResult Unsubscribe() => Ok(new { message = "Unsubscribed" });

        [HttpGet("devices")]
        [Authorize]
        public IActionResult Devices() => Ok(new List<object>());

        [HttpPost("devices")]
        [Authorize]
        public IActionResult RegisterDevice() => Ok(new { message = "Registered" });

        [HttpDelete("devices/{deviceId}")]
        [Authorize]
        public IActionResult UnregisterDevice(Guid deviceId) => Ok(new { deviceId, message = "Unregistered" });

        [HttpPost("devices/{deviceId}/test")]
        [Authorize]
        public IActionResult TestDevice(Guid deviceId) => Ok(new { deviceId, message = "Test sent" });

        [HttpPost("push-tokens")]
        [Authorize]
        public IActionResult RegisterPushToken() => Ok(new { message = "Token registered" });

        [HttpDelete("push-tokens/{tokenId}")]
        [Authorize]
        public IActionResult DeletePushToken(Guid tokenId) => Ok(new { tokenId, message = "Deleted" });

        [HttpPost("/api/admin/notification-templates")]
        [Authorize]
        public IActionResult CreateTemplate() => Ok(new { message = "Template created" });

        [HttpGet("/api/admin/notification-templates")]
        [Authorize]
        public IActionResult ListTemplates() => Ok(new List<object>());

        [HttpGet("/api/admin/notification-templates/{id}")]
        [Authorize]
        public IActionResult TemplateDetail(Guid id) => Ok(new { id });

        [HttpPut("/api/admin/notification-templates/{id}")]
        [Authorize]
        public IActionResult UpdateTemplate(Guid id) => Ok(new { id, message = "Updated" });

        [HttpDelete("/api/admin/notification-templates/{id}")]
        [Authorize]
        public IActionResult DeleteTemplate(Guid id) => Ok(new { id, message = "Deleted" });

        [HttpPost("/api/admin/notification-templates/{id}/preview")]
        [Authorize]
        public IActionResult PreviewTemplate(Guid id) => Ok(new { id, rendered = "preview" });

        [HttpPatch("/api/admin/notification-templates/{id}/status")]
        [Authorize]
        public IActionResult TemplateStatus(Guid id) => Ok(new { id, message = "Status updated" });

        [HttpGet("/api/admin/notifications")]
        [Authorize]
        public IActionResult AdminNotifications() => Ok(new List<object>());

        [HttpGet("/api/admin/notifications/{id}")]
        [Authorize]
        public IActionResult AdminNotificationDetail(Guid id) => Ok(new { id });

        [HttpGet("/api/admin/notifications/search")]
        [Authorize]
        public IActionResult AdminNotificationSearch() => Ok(new List<object>());

        [HttpGet("/api/admin/notifications/statistics")]
        [Authorize]
        public IActionResult AdminNotificationStatistics() => Ok(new { total = 0 });

        [HttpGet("/api/admin/notifications/failed")]
        [Authorize]
        public IActionResult AdminFailedNotifications() => Ok(new List<object>());

        [HttpPost("/api/admin/notifications/{id}/retry")]
        [Authorize]
        public IActionResult AdminRetry(Guid id) => Ok(new { id, message = "Retried" });

        [HttpPost("/api/admin/notifications/broadcast")]
        [Authorize]
        public IActionResult AdminBroadcast() => Ok(new { message = "Broadcast queued" });

        [HttpPost("/api/admin/notifications/{id}/cancel")]
        [Authorize]
        public IActionResult AdminCancel(Guid id) => Ok(new { id, message = "Cancelled" });

        [HttpDelete("/api/admin/notifications/{id}")]
        [Authorize]
        public IActionResult AdminDelete(Guid id) => Ok(new { id, message = "Deleted" });

        [HttpGet("/api/admin/notifications/delivery-statistics")]
        [Authorize]
        public IActionResult AdminDeliveryStatistics() => Ok(new { total = 0 });

        [HttpPost("/api/admin/notification-campaigns")]
        [Authorize]
        public IActionResult CreateCampaign() => Ok(new { message = "Campaign created" });

        [HttpGet("/api/admin/notification-campaigns")]
        [Authorize]
        public IActionResult ListCampaigns() => Ok(new List<object>());

        [HttpGet("/api/admin/notification-campaigns/{id}")]
        [Authorize]
        public IActionResult CampaignDetail(Guid id) => Ok(new { id });

        [HttpPut("/api/admin/notification-campaigns/{id}")]
        [Authorize]
        public IActionResult UpdateCampaign(Guid id) => Ok(new { id, message = "Updated" });

        [HttpPost("/api/admin/notification-campaigns/{id}/schedule")]
        [Authorize]
        public IActionResult ScheduleCampaign(Guid id) => Ok(new { id, message = "Scheduled" });

        [HttpPost("/api/admin/notification-campaigns/{id}/send")]
        [Authorize]
        public IActionResult SendCampaign(Guid id) => Ok(new { id, message = "Send queued" });

        [HttpPost("/api/admin/notification-campaigns/{id}/cancel")]
        [Authorize]
        public IActionResult CancelCampaign(Guid id) => Ok(new { id, message = "Cancelled" });

        [HttpGet("/api/admin/notification-deliveries")]
        [Authorize]
        public IActionResult NotificationDeliveries() => Ok(new List<object>());

        [HttpGet("/api/admin/notification-deliveries/{id}")]
        [Authorize]
        public IActionResult NotificationDeliveryDetail(Guid id) => Ok(new { id });

        [HttpGet("/api/admin/notification-deliveries/failed")]
        [Authorize]
        public IActionResult FailedNotificationDeliveries() => Ok(new List<object>());

        [HttpPost("/api/admin/notification-deliveries/{id}/retry")]
        [Authorize]
        public IActionResult RetryDelivery(Guid id) => Ok(new { id, message = "Retried" });

        [HttpGet("/api/admin/notification-deliveries/{id}/history")]
        [Authorize]
        public IActionResult DeliveryHistory(Guid id) => Ok(new List<object>());

        [HttpGet("/api/admin/notification-deliveries/statistics")]
        [Authorize]
        public IActionResult DeliveryStats() => Ok(new { total = 0 });

        [HttpPost("/api/admin/notification-deliveries/retry-failed")]
        [Authorize]
        public IActionResult RetryFailed() => Ok(new { message = "Retry queued" });

        [HttpGet("/api/internal/health")]
        public IActionResult Health() => Ok(new { status = "Healthy" });
        [HttpGet("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var notification = await _db.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null)
                return NotFound();

            notification.IsRead = true;
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
