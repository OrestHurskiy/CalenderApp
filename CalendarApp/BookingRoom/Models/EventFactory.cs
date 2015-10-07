using BookingRoom.Models.GoogleEvent;
using BookingRoom.Models.GoogleCalendar;
namespace BookingRoom.Models
{
    public class EventFactory
    {
        public CalendarEvent CreateEvent(EventTime start, EventTime end, string summary, string desctiption)
        {
            CalendarEvent newEvent = new CalendarEvent();
            newEvent.Start = start;
            newEvent.End = end;
            newEvent.Summary = summary;
            newEvent.Description = desctiption;
            return newEvent;
        }
    }
}