using BookingRoom.Models.GoogleEvent;

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
