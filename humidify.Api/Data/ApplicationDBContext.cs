using humidify.Core.Models; // <-- CRITICAL: Imports the shared SensorReading model
using Microsoft.EntityFrameworkCore;

namespace humidify.Api.Data
{
    // The AppDbContext handles the connection between your C# models and the PostgreSQL database.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet for the SensorReading model, allowing EF Core to map it to a database table.
        public DbSet<SensorReading> SensorReadings { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the primary key for the SensorReading table
            modelBuilder.Entity<SensorReading>()
                .HasKey(s => s.Id);

            // Optionally set up default seed data or complex mappings here.
            // (Leaving this empty for now, as we'll add data via API endpoints.)

            base.OnModelCreating(modelBuilder);
        }
    }
}