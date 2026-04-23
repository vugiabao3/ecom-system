
// thêm ccái này 
using AuthService.Api.Middlewares;
using AuthService.Application.Interfaces;
using AuthService.Application.Refresh;
using AuthService.Application.Register;
using AuthService.Infrastructure.Data;
    using AuthService.Infrastructure.ExternalServices;
using AuthService.Infrastructure.Fakes;
using AuthService.Infrastructure.RefreshTokens;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.ResetTokens;
using AuthService.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// Add services to the container.
var key = jwtSettings["Key"];

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// thêm cái này 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
});

// thêm cái này để đăng ký service hash password
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
// thêm cái này fake user service


//builder.Services.AddSingleton<IUserServiceClient, FakeUserServiceClient>();

builder.Services.AddHttpClient<IUserServiceClient, UserApiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5183"); // URL UserService
});

//


//thêm 
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
// thêm 
builder.Services.AddSingleton<IResetTokenStore, InMemoryResetTokenStore>();

// thêm 

// thêm 
builder.Services.AddSingleton<IRefreshTokenStore, InMemoryRefreshTokenStore>();

//thêm 
builder.Services.AddScoped<IAuthUserRepository, AuthUserRepository>();



builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auth Service API",
        Version = "v1"
    });

    // 🔥 Thêm JWT vào Swagger
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
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var key = jwtSettings["Key"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,   // ✅ bật luôn cho chuẩn
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            )
        };
    });
//thêm 
builder.Services.AddSingleton<IEmailService, FakeEmailService>();
// hêm 
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

app.Run();
