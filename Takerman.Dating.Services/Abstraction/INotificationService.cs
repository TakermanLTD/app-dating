using Takerman.Dating.Data;
using Takerman.Mail;

namespace Takerman.Dating.Services.Abstraction
{
    public interface INotificationService
    {
        Task NotifyForOrderCreatedAsync(Order order);

        Task NotifyForOrderStatusChangedAsync(Order order);

        Task NotifyForNewsletterAsync(string email);

        Task NotifyForNewMessageAsync(MailMessageDto message);

        Task NotifyForCreatedUser(User user, string domainName);

        Task NotifyForDeletedUser(User user);

        Task NotifyForResetPasswordRequest(string code, User user, string domainName);

        Task NotifyForNotActivatedCreatedUser(User user, string domainName);

        Task NotifyForOrderCanceledAsync(Order result);

        Task SubscribeForNewsletterAsync(Newsletter newsletter);
    }
}