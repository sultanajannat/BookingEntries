using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingEntries.Models
{
    public class Client
    {
        public Client()
        {
            this.bookingEntries = new List<BookingEntry>();
        }
        public int ClientId { get; set; }
        [Required, Display(Name = "Name"), StringLength(80)]
        public string ClientName { get; set; }
        [Required, Display(Name = "Date of Birth"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        [Display(Name = "Married")]
        public bool MaritalStatus { get; set; }

        public ICollection<BookingEntry> bookingEntries { get; set; }

    }
}