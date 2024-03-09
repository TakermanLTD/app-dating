using Microsoft.AspNetCore.SignalR;

namespace Takerman.Dating.Services.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int userId, int toUserId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userId, toUserId, message);
        }
    }
}