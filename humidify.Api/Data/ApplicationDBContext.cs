using humidify.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace humidify.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<SensorReading> SensorReadings { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorReading>()
                .HasKey(s => s.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}