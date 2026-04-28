using Microsoft.IdentityModel.Tokens;
using SharedKernel.Helpers.ErrorHandling;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Cloud Storage Aggregator API",
        Version = "v1"
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddHeaderPropagation(options =>
{
    options.Headers.Add("Authorization");
    options.Headers.Add("Cookie");
});

builder.Services.AddHttpClient("default")
    .AddHeaderPropagation();

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

var app = builder.Build();

app.UseCors("AllowAll");
app.UseGlobalExceptionHandling();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHeaderPropagation();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
