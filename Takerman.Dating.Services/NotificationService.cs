using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;
using Takerman.Mail;

namespace Takerman.Dating.Services
{
    public class NotificationService(IMailService _mailService, IUserService _userService, ILogger<NotificationService> _logger, DefaultContext _context) : INotificationService
    {
        public async Task SubscribeForNewsletterAsync(Newsletter model)
        {
            var newsletter = await _context.Newsletters.FirstOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower());
            var user = await _userService.GetByEmailAsync(model.Email);

            if (newsletter == null)
            {
                newsletter = new Newsletter
                {
                    Email = model.Email,
                    SubscribedOn = DateTime.Now,
                    UserId = user?.Id
                };

                await _context.Newsletters.AddAsync(model);
            }
            else
            {
                newsletter.UserId = user?.Id;
                _context.Newsletters.Update(newsletter);
            }

            await _context.SaveChangesAsync();
        }

        public async Task NotifyForOrderCreatedAsync(Order order)
        {
            var user = await _userService.GetAsync(order.UserId);

            _logger.LogWarning(
@$"A new order has been submitted.

**User**:
User Id: {user.Id}
Name: {user.FirstName} {user.LastName}
Email: {user.Email}

**Order**
Order Id: {order.Id}
Currency: {order.Currency}
Total: {order.Total}
Payment Provider: {order.PaymentSource}
Created On: {order.CreatedOn}");

            await _mailService.SendToQueue(new MailMessageDto()
            {
                From = "tivanov@takerman.net",
                Body =
@$"Thank you for submitting an order to us. Here is the order data. <br /> <br />
<strong>Will be sent to</strong> <br />
Name: {user.FirstName} {user.LastName} <br />
Email: {user.Email} <br />
<br />
<strong>Model</strong> <br />
Total: {order.Total} {order.Currency}<br />
Created On: {order.CreatedOn} <br />",
                To = user.Email,
                Subject = "Sreshti - A new order has been created"
            });
        }

        public async Task NotifyForOrderStatusChangedAsync(Order order)
        {
            var user = await _userService.GetAsync(order.UserId);

            _logger.LogWarning($@"Sreshti - Order status changed Order Id: {order.Id}");

            await _mailService.SendToQueue(new MailMessageDto()
            {
                From = "tivanov@takerman.net",
                Body = @$"Sreshti - Order {order.Id} status changed to {Enum.GetName(order.Status)}. You can login and check it https://Sreshti.com/orders",
                To = user.Email,
                Subject = $"Sreshti - Order {order.Id} status changed to {Enum.GetName(order.Status)}"
            });
        }

        public async Task NotifyForNewsletterAsync(string email)
        {
            _logger.LogWarning($@"Sreshti - A user with email {email} has subscribed to the newsletter.");
        }

        public async Task NotifyForNewMessageAsync(MailMessageDto message)
        {
            await _mailService.SendToQueue(new MailMessageDto()
            {
                Body = $"From: {message.Name}. <br />Email {message.From}. <br />Message: {message.Body}",
                From = "tivanov@takerman.net",
                Subject = $"Sreshti - New email from {message.Name}",
                To = "contact@sreshti.net"
            });
        }

        public async Task NotifyForCreatedUser(User user, string domainName)
        {
            await _mailService.SendToQueue(new MailMessageDto()
            {
                Body = $"You have registered an account in Dating. Please activate. <br />" +
                    $"Here is the link to activate your account: https://{domainName}/activate?code={user.Id} <br /> Login data: </br>" +
                    $"Email: {user.Email} <br /> Password: the password you have set",
                From = "tivanov@takerman.net",
                Subject = "Sreshti - Activate account",
                To = user.Email
            });

            _logger.LogWarning(
@$"A new user has beed registered. It is still not active
Link to [activate](https://{domainName}/activate?code={user.Id})
User Data:
UserId: {user.Id}
Name: {user.FirstName} {user.FirstName}
Email: {user.Email}");
        }

        public async Task NotifyForDeletedUser(User user)
        {
            await _mailService.SendToQueue(new MailMessageDto()
            {
                Body = $"Sorry to see you go. Your account in Sreshti has been deleted completely and all your data with it too. All the best!",
                From = "tivanov@takerman.net",
                Subject = "Sreshti - Account Deleted",
                To = user.Email
            });

            _logger.LogWarning(
@$"An user has deleted his account. User Data:
User Data:
UserId: {user.Id}
Name: {user.FirstName} {user.FirstName}
Email: {user.Email}");
        }

        public async Task NotifyForResetPasswordRequest(string code, User user, string domainName)
        {
            await _mailService.SendToQueue(new MailMessageDto()
            {
                Body = $"You have requested to reset your password. <br />" +
                $"Here is the link to change it: https://{domainName}/reset-password?code={code}&userId={user.Id}",
                From = "tivanov@takerman.net",
                Subject = "Sreshti - Reset password",
                To = user.Email
            });
        }

        public async Task NotifyForOrderCanceledAsync(Order order)
        {
            var user = await _userService.GetAsync(order.UserId);

            await _mailService.SendToQueue(new MailMessageDto()
            {
                Body = $"Your order with number {order.Id} has been canceled and you will receive a full refund of {order.Total} {order.Currency} in couple of days. Please follow your bank statements.<br />",
                From = "tivanov@takerman.net",
                Subject = "Sreshti - Order Canceled",
                To = user.Email
            });

            _logger.LogWarning($"The order with ID {order.Id} has been canceled. Please refund the user.");
        }
    }
}