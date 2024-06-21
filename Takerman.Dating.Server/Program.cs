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
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var hostname = Dns.GetHostName();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!builder.Environment.IsDevelopment())
    connectionString = connectionString.Replace("takerman_dating_bg", hostname);

builder.Services.AddSignalR();
builder.Host.UseSerilog(Log.Logger);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddDbContext<DefaultContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Takerman.Dating.Data")));
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
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
    options.ExcludedHosts.Add(hostname);
    options.ExcludedHosts.Add($"www.{hostname}");
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

app.UseAuthorization();

app.UseExceptionHandler();

app.Use(async (context, next) =>
{
    // context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
    context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
    context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("X-Developed-By", "Takerman Ltd");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
    context.Response.Headers.Add("X-Powered-By", "Pegasus");
    await next();
});

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.MapHub<ChatHub>("/chatHub");

await app.RunAsync();