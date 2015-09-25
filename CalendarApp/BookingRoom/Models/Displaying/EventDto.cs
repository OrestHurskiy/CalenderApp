using BookingRoom.Models.Posting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingRoom.Models.Displaying
{
    public class EventDto : CalendarEvent
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public DateTime? Created { get; set; }
        public string Status { get; set; }
    }
}
