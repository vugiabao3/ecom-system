using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PromotionService.Application.Interfaces;
using PromotionService.Application.Promotions.Commands.ApplyPromotion;
using PromotionService.Application.Promotions.Commands.CreatePromotion;
using PromotionService.Application.Promotions.Commands.DeletePromotion;
using PromotionService.Application.Promotions.Commands.UpdatePromotion;
using PromotionService.Application.Promotions.Queries.GetAllPromotions;
using PromotionService.Infrastructure.Messaging;
using PromotionService.Infrastructure.Persistence;
using PromotionService.Infrastructure.Repositories;
using System.Text;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<PaymentSucceededConsumer>();
builder.Services.AddScoped<IUserPointRepository, UserPointRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ApplyPromotionHandler).Assembly));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Internal";
    options.DefaultChallengeScheme = "Internal";
})
.AddJwtBearer("Internal", options =>
{
    var jwt = builder.Configuration.GetSection("InternalJwt");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,

        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]))
    };
});



builder.Services
    .AddScoped<GetAllPromotionsHandler>();
builder.Services
    .AddScoped<UpdatePromotionHandler>();

builder.Services
    .AddScoped<DeletePromotionHandler>();
builder.Services.AddScoped<CreatePromotionHandler>();
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // 🔐 JWT config
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token dạng: Bearer {token}"
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

var app = builder.Build();

// 🔥 START CONSUMER
using (var scope = app.Services.CreateScope())
{
    var consumer = scope.ServiceProvider.GetRequiredService<PaymentSucceededConsumer>();
    consumer.Start();
}
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // 🔥 PHẢI Ở ĐÂY

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
