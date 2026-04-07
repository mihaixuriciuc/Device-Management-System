using Microsoft.EntityFrameworkCore;
using DeviceManager.Api.Models;

namespace DeviceManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // This property connects your C# 'Device' class to the SQL table
    public DbSet<Device> Devices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // This maps the C# properties to your specific SQL column names
        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Devices");
            entity.Property(e => e.OperatingSystem).HasColumnName("Operating System");
            entity.Property(e => e.OsVersion).HasColumnName("OS version");
            entity.Property(e => e.RamAmount).HasColumnName("RAM amount");
        });
    }
}