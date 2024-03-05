using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;
using Takerman.Mail;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(INotificationService _notificationService) : ControllerBase
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
    }
}