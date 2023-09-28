using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BookingEntries.ViewModels
{

    public class ClientVM
    {
        public ClientVM()
        {
            this.SpotList = new List<int>();
        }
        public int ClientId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ClientName { get; set; }
        [Required, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; }
        [Required]
        public int Age { get; set; }
        public string Picture { get; set; }
        [Required, JsonIgnore, Display(Name = "Image")]
        public HttpPostedFileBase PictureFile { get; set; }
        [Display(Name = "Married")]
        public bool MaritalStatus { get; set; }
        public List<int> SpotList { get; set; }
    }

}