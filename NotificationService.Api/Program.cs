

using NotificationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using NotificationService.Infrastructure.Messaging;
using NotificationService.Infrastructure.Services;


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
                    "http://localhost:5174",
                    "http://localhost:5175"
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

// register service
builder.Services.AddSingleton<NotificationServices>();

builder.Services.AddSingleton<OrderCreatedConsumer>();
builder.Services.AddSingleton<PaymentSucceededConsumer>();
builder.Services.AddSingleton<PaymentFailedConsumer>();
var app = builder.Build();


// start consumer
var consumer = app.Services.GetRequiredService<OrderCreatedConsumer>();
consumer.Start();
var consumer1 = app.Services.GetRequiredService<PaymentSucceededConsumer>();
consumer1.Start();

var failedConsumer = app.Services.GetRequiredService<PaymentFailedConsumer>();
failedConsumer.Start();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
// app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // 🔥 PHẢI Ở ĐÂY


app.UseAuthorization();

app.MapControllers();

app.Run();
