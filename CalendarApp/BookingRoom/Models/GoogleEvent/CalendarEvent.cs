using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Models.GoogleCalendar
{
    public class CalendarEvent
    {
        public EventTime Start { get; set; }
        public EventTime End { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string CalendarID { get; set; }
        
    }
}
