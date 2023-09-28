﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BookingEntries.Models
{
    public class BookingEntry
    {
        public int BookingEntryId { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        [ForeignKey("Spot")]
        public int SpotId { get; set; }

        //Nav
        public virtual Client Client { get; set; }
        public virtual Spot Spot { get; set; }
    }
}