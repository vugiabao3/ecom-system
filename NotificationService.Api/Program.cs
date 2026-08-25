using NotificationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using NotificationService.Infrastructure.Messaging;
using NotificationService.Infrastructure.Services;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);
//cho fe gọi
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5173",
                    "http://127.0.0.1:5173",
                    "http://localhost:5174",
                    "http://127.0.0.1:5174",
                    "http://localhost:5175",
                    "http://127.0.0.1:5175"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// register repository
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// register service
builder.Services.AddSingleton<NotificationServices>();

// register event consumers
builder.Services.AddSingleton<OrderCreatedConsumer>();
builder.Services.AddSingleton<PaymentSucceededConsumer>();
builder.Services.AddSingleton<PaymentFailedConsumer>();
builder.Services.AddSingleton<ShippingCreatedConsumer>();
builder.Services.AddSingleton<DeliveryFailedConsumer>();
builder.Services.AddSingleton<DeliverySucceededConsumer>();
builder.Services.AddSingleton<ReturnOrderConsumer>();

var app = builder.Build();


// start consumers
var orderConsumer = app.Services.GetRequiredService<OrderCreatedConsumer>();
orderConsumer.Start();
var paymentSucceededConsumer = app.Services.GetRequiredService<PaymentSucceededConsumer>();
paymentSucceededConsumer.Start();
var paymentFailedConsumer = app.Services.GetRequiredService<PaymentFailedConsumer>();
paymentFailedConsumer.Start();
var shippingConsumer = app.Services.GetRequiredService<ShippingCreatedConsumer>();
shippingConsumer.Start();
var deliveryFailedConsumer = app.Services.GetRequiredService<DeliveryFailedConsumer>();
deliveryFailedConsumer.Start();
var deliverySucceededConsumer = app.Services.GetRequiredService<DeliverySucceededConsumer>();
deliverySucceededConsumer.Start();
var returnOrderConsumer = app.Services.GetRequiredService<ReturnOrderConsumer>();
returnOrderConsumer.Start();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
// app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // 🔥 PHẢI Ở ĐÂY


app.UseAuthorization();

app.MapControllers();

app.Run();
