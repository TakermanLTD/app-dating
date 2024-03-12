using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Models.Configuration;
using Takerman.Dating.Server.Middleware;
using Takerman.Dating.Services;
using Takerman.Dating.Services.Abstraction;
using Takerman.Dating.Services.Hubs;
using Takerman.Mail;

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
builder.Services.AddDbContext<DefaultContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IDatingService, DatingService>();
builder.Services.AddTransient<IOptionsService, OptionsService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<ICdnService, CdnService>();
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection(nameof(RabbitMqConfig)));
builder.Services.Configure<PayPalConfig>(builder.Configuration.GetSection(nameof(PayPalConfig)));
builder.Services.Configure<CdnConfig>(builder.Configuration.GetSection(nameof(CdnConfig)));

var app = builder.Build();

app.UseDefaultFiles();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.MapHub<ChatHub>("/chatHub");

app.Run();