using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takerman.Dating.Models.Configuration;
using Takerman.Dating.Services;
using Takerman.Dating.Services.Abstraction;
using Takerman.Mail;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Dating.Tests
{
    public class TestFixture : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
            => services
                .AddTransient<IUserService, UserService>()
                .AddTransient<INotificationService, NotificationService>()
                .Configure<RabbitMqConfig>(options => configuration.GetSection(nameof(RabbitMqConfig)).Bind(options))
                .Configure<PayPalConfig>(options => configuration.GetSection(nameof(PayPalConfig)).Bind(options));

        protected override ValueTask DisposeAsyncCore() => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "test-appsettings.json", IsOptional = false };
        }
    }
}