using Takerman.Dating.Data;
using Takerman.Dating.Models.Mailing;

namespace Takerman.Dating.Services.Abstraction
{
    public interface INotificationService
    {
        Task SendEmailAsync(MailMessageDto message);

        Task NotifyForOrderCreatedAsync(Order order);

        Task NotifyForOrderStatusChangedAsync(Order order);

        Task NotifyForNewsletterAsync(string email);

        Task NotifyForNewMessageAsync(Message message);

        Task NotifyForCreatedUser(User user, string domainName);

        Task NotifyForDeletedUser(User user);

        Task NotifyForResetPasswordRequest(string code, User user, string domainName);

        Task NotifyForOrderCanceledAsync(Order result);

        Task SubscribeForNewsletterAsync(Newsletter newsletter);
    }
}