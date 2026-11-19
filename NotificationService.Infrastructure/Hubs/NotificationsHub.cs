using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace NotificationService.Infrastructure.Hubs
{
    [Authorize]
    public class NotificationsHub : Hub
    {
        private static string GetUserId(ClaimsPrincipal? user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user?.FindFirst("sub")?.Value
                ?? string.Empty;
        }

        public override async Task OnConnectedAsync()
        {
            await JoinUserGroup();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetUserId(Context.User);
            if (!string.IsNullOrEmpty(userId))
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            await base.OnDisconnectedAsync(exception);
        }

        public Task JoinGroup() => JoinUserGroup();

        private async Task JoinUserGroup()
        {
            var userId = GetUserId(Context.User);
            
            if (!string.IsNullOrEmpty(userId))
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
    }
}
