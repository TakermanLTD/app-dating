using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;
using Takerman.Mail;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(INotificationService _notificationService, IChatService _chatService) : ControllerBase
    {
        [HttpPost("SendContactUsMessage")]
        public async Task SendContactUsMessage([FromBody] MailMessageDto model)
        {
            await _notificationService.NotifyForNewMessageAsync(model);
        }

        [HttpPost("SubscripeToNewsletter")]
        public async Task SubscripeToNewsletter(Newsletter newsletter)
        {
            await _notificationService.SubscribeForNewsletterAsync(newsletter);
        }

        [HttpGet("GetChatMessagesAsync")]
        public Task<IEnumerable<ChatMessageDto>> GetChatMessagesAsync(int userId, int toUserId)
        {
            return _chatService.GetChatMessagesAsync(userId, toUserId);
        }

        [HttpPost("SendChatMessageAsync")]
        public async Task SendChatMessageAsync(ChatMessageDto message)
        {
            await _chatService.SendChatMessageAsync(message);
        }
    }
}