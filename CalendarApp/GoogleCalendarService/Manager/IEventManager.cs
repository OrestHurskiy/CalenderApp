using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;

namespace GoogleCalendarService.Manager
{

    public interface IEventManager
    {
        List<Event> CheckEvent(string calendarId, Event eventToAdd);
    }
}
