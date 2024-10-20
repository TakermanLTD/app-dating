using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Data.Initialization;
using Takerman.Dating.Models.Configuration;
using Takerman.Dating.Server.Middleware;
using Takerman.Dating.Services;
using Takerman.Dating.Services.Abstraction;
using Takerman.Dating.Services.Hubs;
using Takerman.Mail;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Security.Claims;
using Takerman.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var hostname = Dns.GetHostName();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
if (!builder.Environment.IsDevelopment())
    connectionString = connectionString.Replace("takerman_dating_bg", hostname);

var googleSection = builder.Configuration.GetSection(nameof(GoogleAuthConfig));
var facebookSection = builder.Configuration.GetSection(nameof(FacebookAuthConfig));

builder.Host.AddTakermanLogging();
builder.Logging.AddTakermanLogging();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
builder.Services.AddDbContext<DefaultContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Takerman.Dating.Data")));
builder.Services.AddTransient<DbContext, DefaultContext>();
builder.Services.AddTransient<IContextInitializer, ContextInitializer>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IDatingService, DatingService>();
builder.Services.AddTransient<IOptionsService, OptionsService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<ICdnService, CdnService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection(nameof(RabbitMqConfig)));
builder.Services.Configure<PayPalConfig>(builder.Configuration.GetSection(nameof(PayPalConfig)));
builder.Services.Configure<CdnConfig>(builder.Configuration.GetSection(nameof(CdnConfig)));
builder.Services.Configure<GoogleAuthConfig>(googleSection);
builder.Services.Configure<FacebookAuthConfig>(facebookSection);
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
    options.ExcludedHosts.Add(hostname);
    options.ExcludedHosts.Add($"www.{hostname}");
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
}).AddCookie()
.AddGoogle(options =>
{
    options.ClientId = googleSection["ClientId"] ?? string.Empty;
    options.ClientSecret = googleSection["ClientSecret"] ?? string.Empty;
    options.Events.OnCreatingTicket = context =>
    {
        context.Identity.AddClaim(new Claim("access_token", context.AccessToken));
        if (context.RefreshToken != null)
        {
            context.Identity.AddClaim(new Claim("refresh_token", context.RefreshToken));
        }
        return Task.CompletedTask;
    };
})
.AddFacebook(options =>
{
    options.AppId = facebookSection["ClientId"] ?? string.Empty;
    options.AppSecret = facebookSection["ClientSecret"] ?? string.Empty;
    options.Scope.Add("id");
    options.Scope.Add("name");
    options.Scope.Add("email");
    options.Scope.Add("gender");
    options.Scope.Add("hometown");
    options.Scope.Add("link");
    options.Scope.Add("photos");
    options.Fields.Add("id");
    options.Fields.Add("name");
    options.Fields.Add("email");
    options.Fields.Add("gender");
    options.Fields.Add("hometown");
    options.Fields.Add("link");
    options.Fields.Add("photos");
    options.Events.OnCreatingTicket = context =>
    {
        context.Identity.AddClaim(new Claim("access_token", context.AccessToken));
        if (context.RefreshToken != null)
        {
            context.Identity.AddClaim(new Claim("refresh_token", context.RefreshToken));
        }
        return Task.CompletedTask;
    };
});

var app = builder.Build();

using var scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<IContextInitializer>().InitializeAsync();

app.UseDefaultFiles();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

app.Use(async (context, next) =>
{
    // context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
    context.Response.Headers.TryAdd("Cache-Control", "no-cache, no-store, must-revalidate");
    context.Response.Headers.TryAdd("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
    context.Response.Headers.TryAdd("Referrer-Policy", "no-referrer");
    context.Response.Headers.TryAdd("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
    context.Response.Headers.TryAdd("X-Developed-By", "Takerman Ltd");
    context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    context.Response.Headers.TryAdd("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.TryAdd("X-Permitted-Cross-Domain-Policies", "none");
    context.Response.Headers.TryAdd("X-Powered-By", "Takerman");
    await next();
});

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.MapHub<ChatHub>("/chatHub");

await app.RunAsync();