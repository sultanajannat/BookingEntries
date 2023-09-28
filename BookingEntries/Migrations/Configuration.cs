using System.Collections.Generic;
using BookingEntries.Models;

namespace BookingEntries.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BookingEntries.Models.BookingDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BookingEntries.Models.BookingDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            List<Spot> spots = new List<Spot>()
            {
                new Spot(){SpotId = 1, SpotName = "Sundarban"},
                new Spot(){SpotId = 2, SpotName = "Srimagal"},
                new Spot(){SpotId = 3, SpotName = "Jaflong"},
                new Spot(){SpotId = 4, SpotName = "Ratatgul Swamp Forest"},
                new Spot(){SpotId = 5, SpotName = "Inani Beach"},
                new Spot(){SpotId = 6, SpotName = "Sitakundu"},
                new Spot(){SpotId = 7, SpotName = "Sajek Valley"}
            };

            context.Spots.AddRange(spots);
        }
    }
}
