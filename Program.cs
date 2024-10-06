/*
 * File: Program.cs
 * Description: Configures and initializes the ECommerce Backend API application.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024/09/29
 */


using System.Text;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.Data.Repository.Implementations;
using ECommerceBackend.Data.Repository.Interfaces;
using ECommerceBackend.Models;
using ECommerceBackend.Service.Implementations;
using ECommerceBackend.Service.Implementations;
using ECommerceBackend.Service.Interfaces;
using ECommerceBackend.Service.Interfaces;
using ECommerceBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Logging configuration
// Clears existing logging providers and adds console logging with Debug level
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Database settings configuration
// Configures database settings from appsettings.json and registers MongoDbContext as a singleton
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(nameof(DatabaseSettings))
);
builder.Services.AddSingleton<MongoDbContext>();

// Register services
// Adds scoped services for authentication and user management
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserServices, UserService>();
builder.Services.AddScoped<IBloblService, BlobService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Add controllers
// Registers MVC controllers for handling HTTP requests
builder.Services.AddControllers();

// Swagger configuration for API documentation
// Sets up Swagger with JWT authentication support
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce Backend API", Version = "v1" });
    option.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer",
        }
    );
    option.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

// Configure JWT settings
// Retrieves and validates JWT settings from configuration
var jwtSettingsSection = builder.Configuration.GetSection(nameof(JwtSettings));
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
if (
    jwtSettings == null
    || string.IsNullOrEmpty(jwtSettings.Secret)
    || string.IsNullOrEmpty(jwtSettings.Issuer)
    || string.IsNullOrEmpty(jwtSettings.Audience)
)
{
    throw new Exception("JWT settings are not configured properly in appsettings.json.");
}

// Retrieve JWT parameters from configuration
var secret = builder.Configuration["JwtSettings:Secret"];
var key = Encoding.UTF8.GetBytes(secret);
var issuer = builder.Configuration["JwtSettings:Issuer"];
var audience = builder.Configuration["JwtSettings:Audience"];

// JWT Authentication configuration
// Sets up JWT Bearer authentication with token validation parameters
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.UseSecurityTokenValidators = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidAudience = audience,
            ValidIssuer = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.FromMinutes(5),
        };
    });

// Authorization policies
// Defines default and custom authorization policies for the application
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));
    options.AddPolicy("CSRPolicy", policy => policy.RequireRole("CSR"));
    options.AddPolicy("VendorPolicy", policy => policy.RequireRole("Vendor"));
    options.AddPolicy("VendorOrAdminPolicy", policy => policy.RequireRole("Vendor", "Admin"));
    options.AddPolicy("AdminOrCSRPolicy", policy => policy.RequireRole("CSR", "Admin"));
});

// Register CORS policy
// Configures Cross-Origin Resource Sharing to allow any origin, method, and header
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

// Health Checks
// Adds health check services to monitor the application's health
builder.Services.AddHealthChecks();

var app = builder.Build();

// Use HTTPS Redirection
// Redirects HTTP requests to HTTPS
app.UseHttpsRedirection();

// Use CORS
// Applies the configured CORS policy to incoming requests
app.UseCors("CorsPolicy");

// Swagger integration in Development mode
// Enables Swagger UI only in the Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use Authentication and Authorization
// Enables authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers and Health Check endpoints
// Maps controller routes and the health check endpoint
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
