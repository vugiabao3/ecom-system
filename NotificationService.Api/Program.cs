using NotificationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NotificationService.Infrastructure.Messaging;
using NotificationService.Infrastructure.Services;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173","http://127.0.0.1:5173","http://localhost:5174","http://127.0.0.1:5174","http://localhost:5175","http://127.0.0.1:5175")
              .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my-super-secret-key-12345678901234567890"))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
                    context.Token = accessToken;
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddSingleton<NotificationServices>();
builder.Services.AddSingleton<OrderCreatedConsumer>();
builder.Services.AddSingleton<PaymentSucceededConsumer>();
builder.Services.AddSingleton<PaymentFailedConsumer>();
builder.Services.AddSingleton<ShippingCreatedConsumer>();
builder.Services.AddSingleton<DeliveryFailedConsumer>();
builder.Services.AddSingleton<DeliverySucceededConsumer>();
builder.Services.AddSingleton<ReturnOrderConsumer>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationService.Infrastructure.Hubs.NotificationsHub>("/hubs/notifications");
app.Services.GetRequiredService<OrderCreatedConsumer>().Start();
app.Services.GetRequiredService<PaymentSucceededConsumer>().Start();
app.Services.GetRequiredService<PaymentFailedConsumer>().Start();
app.Services.GetRequiredService<ShippingCreatedConsumer>().Start();
app.Services.GetRequiredService<DeliveryFailedConsumer>().Start();
app.Services.GetRequiredService<DeliverySucceededConsumer>().Start();
app.Services.GetRequiredService<ReturnOrderConsumer>().Start();
app.Run();
