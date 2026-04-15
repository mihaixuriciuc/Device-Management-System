using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using DeviceManager.Api.Models;

namespace DeviceManager.Api.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

   
    public DbSet<Device> Devices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CRITICAL: This base call configures all the security tables (AspNetUsers, etc.)
        // It MUST stay right here before your custom configurations.
        base.OnModelCreating(modelBuilder);

        // Your existing custom mappings stay exactly as they were!
        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Devices");
            entity.Property(e => e.OperatingSystem).HasColumnName("OperatingSystem");
            entity.Property(e => e.OsVersion).HasColumnName("OSversion");
            entity.Property(e => e.RamAmount).HasColumnName("RAMamount");
        });
    }
}