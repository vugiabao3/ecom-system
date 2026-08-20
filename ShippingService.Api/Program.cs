using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShippingService.Application.Interfaces;
using ShippingService.Application.Shipments.Commands.CreateShipment;
using ShippingService.Infrastructure.Messaging;
using ShippingService.Infrastructure.Persistence;
using ShippingService.Infrastructure.Repositories;
using ShippingService.Infrastructure.Security;
using ShippingService.Infrastructure.Service;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var key = Encoding.UTF8.GetBytes("my-super-secret-key-12345678901234567890");




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

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

builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Shipping API",
        Version = "v1"
    });

    // 🔥 THÊM JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập token dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



builder.Services.AddScoped<IEventBus, RabbitMqEventBus>();

builder.Services.AddScoped<CreateShipmentHandler>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


builder.Services.AddSingleton<ServiceTokenGenerator>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new ServiceTokenGenerator(config["ServiceAuth:Secret"]);
});
//builder.Services.AddSingleton<ShippingCreatedConsumer>();
builder.Services.AddHttpClient<IOrderServiceClient, OrderServiceClient>();
builder.Services.AddSingleton<PaymentSucceededConsumer>(); builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateShipmentHandler).Assembly));
var app = builder.Build();


var consumer = app.Services.GetRequiredService<PaymentSucceededConsumer>();
consumer.Start();

//app.Services.GetRequiredService<ShippingCreatedConsumer>().Start();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // 🔥 PHẢI Ở ĐÂY

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
