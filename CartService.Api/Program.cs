
using CartService.Application.Cart.Commands.AddToCart;
using CartService.Application.Cart.EventHandlers;
using CartService.Application.Events;
using CartService.Application.Interfaces;
using CartService.Infrastructure.Persistence;
using CartService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using MediatR;
using CartService.Infrastructure.Events;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập token: Bearer {your token}"
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
}); builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AddToCartHandler).Assembly);
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ProductDeletedEventHandler>();
builder.Services.AddSingleton<ProductDeletedConsumer>();



builder.Services.AddHttpClient<
    IInventoryServiceClient,
    InventoryServiceClient>(client =>
    {
        client.BaseAddress =
            new Uri("http://inventoryservice:8080");
    });
builder.Services.AddScoped<ProductUpdatedEventHandler>();
builder.Services.AddSingleton<ProductUpdatedConsumer>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,   // dev thì để false
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("my-super-secret-key-12345678901234567890")) // ⚠️ phải giống AuthService
    };
});

var app = builder.Build();

var consumer = app.Services.GetRequiredService<ProductUpdatedConsumer>();
consumer.Start();
var consumer2 = app.Services.GetRequiredService<ProductDeletedConsumer>();
consumer2.Start();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // 🔥 PHẢI Ở ĐÂY


app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

app.Run();
