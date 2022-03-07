using Microsoft.EntityFrameworkCore;

namespace CSC237_TripLog_start1.Models
{
    public class TripLogContext : DbContext
    {
        public TripLogContext(DbContextOptions<TripLogContext> options)
            : base(options)
        { }

        public DbSet<Trip> Trips { get; set; }
    }
}
