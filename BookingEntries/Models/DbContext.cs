using System.Data.Entity;

namespace BookingEntries.Models
{
    public class BookingDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<BookingEntry> BookingEntries { get; set; }
    }
}
