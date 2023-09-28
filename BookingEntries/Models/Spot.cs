using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingEntries.Models
{
    public class Spot
    {
        public Spot()
        {
            this.bookingEntries = new List<BookingEntry>();
        }
        public int SpotId { get; set; }
        [Required, Display(Name = "Name"), StringLength(80)]
        public string SpotName { get; set; }

        public ICollection<BookingEntry> bookingEntries { get; set; }
    }
}