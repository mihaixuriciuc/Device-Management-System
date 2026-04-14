using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DeviceManager.Api.Data;
using DeviceManager.Api.Models;
using DeviceManager.Api.Services;



var builder = WebApplication.CreateBuilder(args);

// --- 1. CORE SERVICES ---
builder.Services.AddControllers();


// --- 2. DATABASE & IDENTITY ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity to use our ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// --- 3. AUTHENTICATION & JWT ---
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "Your_Super_Secret_Fallback_Key_32_Chars");

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

// --- 4. CORS & DEPENDENCY INJECTION ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Must be the exact Angular URL!
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // <--- CRITICAL FOR COOKIES
    });
});

// Register your services
builder.Services.AddScoped<DeviceServiceInterface, DeviceService>();
// builder.Services.AddScoped<IAuthService, AuthService>(); // Add this once you create AuthService
// Inside the Dependency Injection section
builder.Services.AddScoped<AuthServiceInterface,AuthService>();

var app = builder.Build();


app.UseCors("AllowAngularApp");

// CRITICAL: Authentication MUST come before Authorization
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

// --- ADMIN SEEDER SCRIPT ---
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 1. Create the Admin Role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // 2. Create the First Admin User if they don't exist
    var adminEmail = "admin@devicemanager.com";
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

        var result = await userManager.CreateAsync(adminUser, "SuperSecretAdmin123!");
        
        if (result.Succeeded)
        {
            // 3. Attach the Admin role to this user
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

app.Run();

public partial class Program { }