using Takerman.Dating.Services.Abstraction;
using Takerman.Mail;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Dating.Tests
{
    public class MailTests : TestBed<TestFixture>
    {
        private const string DOMAIN = "https://localhost:5147";
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public MailTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _userService = _fixture.GetService<IUserService>(_testOutputHelper);
            _notificationService = _fixture.GetService<INotificationService>(_testOutputHelper);
        }

        [Theory]
        [InlineData("tivanov@takerman.net")]
        public async Task Should_SendResetPasswordEmail_When_CorrectInputIsPassed(string email)
        {
            var user = _userService.GetByEmailAsync(email);

            var actual = await _userService.GenerateResetPasswordRequest(user.Id);

            Assert.NotNull(actual);
        }

        [Theory]
        [InlineData("tivanov@takerman.net")]
        public async Task Should_SendContactUsEmail_When_CorrectInputIsPassed(string email)
        {
            var model = new MailMessageDto()
            {
                Body = $"From: Dating Tests. <br />Email {email}. <br />Message: This is test email sent from the tests of Dating",
                From = email,
                Subject = "New email from the contact form of Dating website",
                To = "contact@sreshti.net"
            };

            await Assert.ThrowsAsync<Exception>(async () => await _notificationService.NotifyForNewMessageAsync(model));
        }
    }
}