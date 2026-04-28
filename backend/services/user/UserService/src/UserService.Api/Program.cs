using AuthService.Application.Options;
using AuthService.Application.Services.JWT;
using AuthService.Application.Repositories;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services.JWTService;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using UserService.Application.Messaging;
using UserService.Infrastructure.Messaging;
using UserService.Application.CQRS.Command;
using UserService.Application.Repositories;
using UserService.Infrastructure.Repositories;
using UserService.Application.Services;
using UserService.Infrastructure.Services;
using SharedKernel.Services.Email;
using UserService.Application.CQRS.Query;
using SharedKernel.Helpers.ErrorHandling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(RegisterUserCommand).Assembly,
        typeof(SearchUsersQuery).Assembly));


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService,SmtpEmailService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddHttpClient<IStorageApiClient, StorageApiClient>();


builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});




var app = builder.Build();

app.UseCors("AllowAll");

app.UseGlobalExceptionHandling();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
