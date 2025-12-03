using Backend.API.IAM.Application.Internal.CommandServices;
using Backend.API.IAM.Application.Internal.OutboundServices;
using Backend.API.IAM.Application.Internal.QueryServices;
using Backend.API.IAM.Domain.Repositories;
using Backend.API.IAM.Domain.Services;
using Backend.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using Backend.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using Backend.API.IAM.Infrastructure.Pipeline.Middleware.Components;
using Backend.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using Backend.API.IAM.Infrastructure.Tokens.JWT.Services;
using Backend.API.Shared.Domain.Repositories;
using Backend.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Backend.API.Groups.Domain.Repositories;
using Backend.API.Groups.Domain.Services;
using Backend.API.Groups.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 23))));

// IAM Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Groups Services
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IColleagueRepository, ColleagueRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<ICalculationRepository, CalculationRepository>();
builder.Services.AddScoped<IDistanceCalculationService, DistanceCalculationService>();

// Token Settings Configuration
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

// Authentication
var tokenSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>();
if (tokenSettings == null || string.IsNullOrEmpty(tokenSettings.Secret))
{
    throw new InvalidOperationException("TokenSettings.Secret is not configured in appsettings.json");
}

var key = Encoding.ASCII.GetBytes(tokenSettings.Secret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LocalFood API",
        Version = "v1",
        Description = "Learning Center Platform API"
    });

    // Add JWT authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            new string[] { }
        }
    });
});

// Add controllers
builder.Services.AddControllers();

// CORS configuration - Simple and permissive for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "LocalFood API v1");
    options.RoutePrefix = "swagger";
    options.DefaultModelsExpandDepth(1);
    options.DefaultModelExpandDepth(1);
});

// CORS MUST be early
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Custom middleware for request authorization - after auth/authz
app.UseMiddleware<RequestAuthorizationMiddleware>();

app.MapControllers();


// Database migration and seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();
