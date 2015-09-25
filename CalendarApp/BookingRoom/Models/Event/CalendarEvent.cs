using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Models.Posting
{
    public class CalendarEvent
    {
        public EventTime Start { get; set; }
        public EventTime End { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string CalendarID { get; set; }
        public Event ToEvent()
        {
            Event toAddEvent = new Event();
            toAddEvent.Summary = Summary;
            toAddEvent.Description = Description;
            toAddEvent.Start = new EventDateTime()
            {
                DateTime = Start.ToDateTime()
            };
            toAddEvent.End = new EventDateTime()
            {
                DateTime = End.ToDateTime()
            };
            return toAddEvent;
        }
    }
}