using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DeviceManager.Api.Data;
using DeviceManager.Api.Models;
using DeviceManager.Api.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// --- Core Services ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddHttpClient();

// --- Database & Identity ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// --- JWT + Cookie Authentication ---
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    // Read JWT from HttpOnly cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("AccessToken"))
            {
                context.Token = context.Request.Cookies["AccessToken"];
            }
            return Task.CompletedTask;
        }
    };
});

// CORS (important for Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Services
builder.Services.AddScoped<AiServiceInterface, AiService>();
builder.Services.AddScoped<DeviceServiceInterface, DeviceService>();
builder.Services.AddScoped<AuthServiceInterface, AuthService>();

var app = builder.Build();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed roles + admin user
// === Role & Admin Seeding (Clean & Single Block) ===
// === Role & Admin Seeding - Clean & Safe Version ===
// === Role & Admin Seeding - Clean & Configurable ===
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    // Seed basic roles
    string[] roles = { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed default Admin user from configuration
    var adminEmail = config["AdminSettings:Email"] ?? "admin@devicemanager.com";
    var adminPassword = config["AdminSettings:Password"] ?? "SuperSecretAdmin123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "System",
            LastName = "Admin",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
            Console.WriteLine($"✅ Default Admin user created: {adminEmail}");
        }
    }
    else
    {
        Console.WriteLine($"ℹ️  Admin user already exists: {adminEmail}");
    }
}
// === Global Exception Handler (Clean & Professional) ===
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var error = new
        {
            message = "An unexpected error occurred.",
            statusCode = context.Response.StatusCode
        };

        // In Development, show more details (helpful for debugging)
        if (context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            var exceptionHandlerPathFeature = 
                context.Features.Get<IExceptionHandlerPathFeature>();

            var ex = exceptionHandlerPathFeature?.Error;

            await context.Response.WriteAsJsonAsync(new
            {
                message = ex?.Message ?? "Internal Server Error",
                details = ex?.StackTrace,
                statusCode = context.Response.StatusCode
            });
        }
        else
        {
            await context.Response.WriteAsJsonAsync(error);
        }
    });
});

app.Run();

public partial class Program { }