using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Helpers.ErrorHandling;
using Storage.API.BackgroundServices;
using Storage.Application.CQRS.Command.Folders.UploadFolderCommand;
using Storage.Application.CQRS.Query;
using Storage.Application.Messaging;
using Storage.Application.Repositories;
using Storage.Application.Services;
using Storage.Infrastructure.Messaging;
using Storage.Infrastructure.Repositories;
using Storage.Infrastructure.Services;
using Storage.Infrastructure.Services.Storage.Local;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var profilesPath = Path.Combine(Directory.GetCurrentDirectory(), "storage-data", "profiles");


builder.Services.AddControllers();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
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


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CreateFolderCommand).Assembly,
    typeof(GetFolderByUserIdQuery).Assembly));

builder.Services.AddHttpClient<IPermissionApiClient, PermissionApiClient>(c =>
{
    c.BaseAddress = new Uri("https://localhost:7165"); 
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

builder.Services.AddScoped<IUserStorageCleanupService, UserStorageCleanupService>();

builder.Services.AddHostedService<UserDeletedConsumer>();




var app = builder.Build();

app.UseCors("AllowAll");

app.UseGlobalExceptionHandling();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(profilesPath),
    RequestPath = "/profiles",
    ServeUnknownFileTypes = true
});

app.Run();
