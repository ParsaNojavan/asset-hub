using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Helpers.ErrorHandling;
using Sharing.API.BackgroundServices;
using Sharing.Application.CQRS.Command;
using Sharing.Application.CQRS.Query;
using Sharing.Application.Repositories;
using Sharing.Application.Services;
using Sharing.Infrastructure.Messaging;
using Sharing.Infrastructure.Repositories;
using Sharing.Infrastructure.Services;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ISharedAssetRepository, SharedAssetRepository>();
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CheckPermissionQuery).Assembly,
    typeof(CreateShareCommand).Assembly
    )
);

builder.Services.AddHttpClient<IStorageServiceClient, StorageServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7024");
});


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

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddScoped<ISharedUserCleanUpService, SharedUserCleanUpService>();

builder.Services.AddHostedService<AssetDeletedConsumer>();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseGlobalExceptionHandling();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
