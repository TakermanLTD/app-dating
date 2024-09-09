using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Data.Initialization;
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
                .Configure<AppSettings>(configuration.GetSection("AppSettings"))
                .AddDbContext<DefaultContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Takerman.Dating.Data")))
                .AddTransient<DbContext, DefaultContext>()
                .AddTransient<IContextInitializer, ContextInitializer>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<INotificationService, NotificationService>()
                .AddTransient<IOrderService, OrderService>()
                .AddTransient<IDatingService, DatingService>()
                .AddTransient<IOptionsService, OptionsService>()
                .AddTransient<IMailService, MailService>()
                .AddTransient<IChatService, ChatService>()
                .AddTransient<ICdnService, CdnService>()
                .AddTransient<IAdminService, AdminService>()
                .Configure<RabbitMqConfig>(configuration.GetSection(nameof(RabbitMqConfig)))
                .Configure<PayPalConfig>(configuration.GetSection(nameof(PayPalConfig)))
                .Configure<CdnConfig>(configuration.GetSection(nameof(CdnConfig)));

        protected override ValueTask DisposeAsyncCore() => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            var result = new List<TestAppSettings>()
            {
                new(){ Filename = "test-appsettings.json", IsOptional = false },
                new(){ Filename = "test-appsettings.Production.json", IsOptional = true }
            };

            return result;
        }
    }
}