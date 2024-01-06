using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService _notificationService = notificationService;

        [HttpPost("SendContactUsMessage")]
        public async Task SendContactUsMessage([FromBody] Message model)
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