using humidify.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace humidify.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // These lines create the tables in your database
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<UserAction> UserActions { get; set; }
    }
}