using System.Text;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.Data.Repository.Implementations;
using ECommerceBackend.Data.Repository.Interfaces;
using ECommerceBackend.Models;
using ECommerceBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Set to Debug for detailed logs

// Configure DatabaseSettings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(nameof(DatabaseSettings))
);
builder.Services.AddSingleton<MongoDbContext>();

// Register the AuthService and other services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserServices, UserService>();

// Add controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JwtSettings
var jwtSettingsSection = builder.Configuration.GetSection(nameof(JwtSettings));
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

// Check JwtSettings here
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
if (jwtSettings == null)
{
    throw new Exception("JWT settings are not configured properly in appsettings.json.");
}

if (
    string.IsNullOrEmpty(jwtSettings.Secret)
    || string.IsNullOrEmpty(jwtSettings.Issuer)
    || string.IsNullOrEmpty(jwtSettings.Audience)
)
{
    throw new Exception("JWT settings are missing required properties.");
}

var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

// Configure JWT authentication
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ClockSkew = TimeSpan.Zero,
        };
    });

// Configure the default authorization policy
builder.Services.AddAuthorization(options =>
{
    // Default policy requires authentication
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

    // Define role-based policy for Admin
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

// Configure Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
