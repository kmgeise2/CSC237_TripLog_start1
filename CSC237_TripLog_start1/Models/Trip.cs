using System;
using System.ComponentModel.DataAnnotations;

namespace CSC237_TripLog_start1.Models
{
    public class Trip
    {
        // EF Core will configure the database to generate this value
        // See page 136 in Murach textbook
        public int TripId { get; set; } //Primary key

        [Required(ErrorMessage = "Please enter a destination.")]
        public string Destination { get; set; }

        [Required(ErrorMessage = "Please enter the date your trip starts.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please enter the date your trip ends.")]
        public DateTime? EndDate { get; set; }

        //Accommodation is optional
        public string Accommodation { get; set; }
        public string AccommodationPhone { get; set; }
        public string AccommodationEmail { get; set; }

        // Things to do are optional
        public string ThingToDo1 { get; set; }
        public string ThingToDo2 { get; set; }
        public string ThingToDo3 { get; set; }
    }
}
