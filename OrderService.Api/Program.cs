using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders.Commands.CreateOrder;
using OrderService.Application.Orders.EventHandlers;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Security;
using OrderService.Infrastructure.Services;
using System.Reflection.Metadata;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;



// 🔥 1. Add Controllers
builder.Services.AddControllers();

// 🔥 2. Swagger + JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddScoped<PaymentSucceededEventHandler>();
builder.Services.AddScoped<PaymentFailedEventHandler>();

builder.Services.AddSingleton<PaymentConsumer>();

// 🔥 DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔥 MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly));


// 🔥 5. Repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddHttpContextAccessor();

// 🔥 6. HttpClient (Cart + Product)
builder.Services.AddHttpClient<ICartServiceClient, CartServiceClient>();
builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>();

// 🔥 7. CurrentUser (lấy từ JWT)
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<ServiceTokenGenerator>();
// 🔥 8. EventBus (RabbitMQ)
builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();

builder.Services.AddHttpClient<IPromotionClient, PromotionClient>();
// 🔥 9. Authentication (JWT)

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "User";     // 🔥 mặc định cho API user
    options.DefaultChallengeScheme = "User";
})
.AddJwtBearer("User", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        NameClaimType = "sub" // 🔥 QUAN TRỌNG NHẤT
    };
})
.AddJwtBearer("Internal", options =>
{
    var jwt = builder.Configuration.GetSection("InternalJwt");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]))
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
                    "http://localhost:5174",
                    "http://localhost:5175"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("InternalOnly", policy =>
        policy.AddAuthenticationSchemes("Internal")
              .RequireAuthenticatedUser());

    options.AddPolicy("UserOnly", policy =>
        policy.AddAuthenticationSchemes("User")
              .RequireAuthenticatedUser());
});
builder.Services.AddAuthorization();

// ===================== BUILD =====================

var app = builder.Build();
var consumer = app.Services.GetRequiredService<PaymentConsumer>();
consumer.Start();



var scope = app.Services.CreateScope();
var gen = scope.ServiceProvider.GetRequiredService<ServiceTokenGenerator>();

Console.WriteLine("🔥 INTERNAL JWT:");
Console.WriteLine(gen.Generate());
// 🔥 10. Swagger

app.UseSwagger();
app.UseSwaggerUI();


// 🔥 11. Middleware
// app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // 🔥 PHẢI Ở ĐÂY


app.UseAuthentication(); // 🔥 QUAN TRỌNG
app.UseAuthorization();

app.MapControllers();

app.Run();