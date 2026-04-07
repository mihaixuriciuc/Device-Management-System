using Microsoft.EntityFrameworkCore;
using DeviceManager.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllers(); // THIS IS CRUCIAL - It tells the app to look for your Controllers folder
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Connect to the Database (using the name from appsettings.json)
builder.Services.AddControllers(options =>
{
    options.AllowEmptyInputInBodyModelBinding = false;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. THIS WAS MISSING: Connect to the Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 1. Define the "Allow Angular" Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200") // The address of your Angular app
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowAngularApp");

// 3. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// We commented this out earlier to avoid the HTTPS warning
// app.UseHttpsRedirection();

app.UseAuthorization();

// 4. Map the Controllers (This is the "On" switch for your DevicesController)
app.MapControllers();

app.Run();

public partial class Program { }