using Microsoft.EntityFrameworkCore;
using Serilog;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Models.Broker;
using Takerman.Dating.Server.Middleware;
using Takerman.Dating.Services;
using Takerman.Dating.Services.Abstraction;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog(Log.Logger);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddDbContext<DefaultContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDatingService, DatingService>();
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection(nameof(RabbitMqConfig)));
builder.Services.Configure<PayPalConfig>(builder.Configuration.GetSection(nameof(PayPalConfig)));
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

app.Run();