using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;

namespace GoogleCalendarService
{
    public interface IBookingService
    {
        Event UpdateEvent(Event eventToUpdate, string calendarId, string eventId);
        Event PostEvent(Event eventToAdd, string calendarId);
        List<Event> GetEvents(string calendarId);
        void DeleteEvent(string calendarId, string eventId);
        Event SearchEventById(string calendarId, string eventId);
        string GetCalendarTimeZone(string calendarId);
    }
}