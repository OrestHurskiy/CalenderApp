using BookingRoom.Models.GoogleCalendar;
using Google.Apis.Calendar.v3.Data;

namespace BookingRoom.Models.GoogleEvent
{
    public static class ToEventConverter
    {
        public static Event ToEvent(CalendarEvent eventForConverting)
        {
            Event newEvent = new Event
            {
                Summary = eventForConverting.Summary,
                Description = eventForConverting.Description,
                Start = new EventDateTime
                {
                    DateTime = eventForConverting.Start.ToDateTime()
                },
                End = new EventDateTime()
                {
                    DateTime = eventForConverting.End.ToDateTime()
                }
            };
            return newEvent;
        }
    }
}