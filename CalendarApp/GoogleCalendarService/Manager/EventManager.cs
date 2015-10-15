using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService.Extensions;

namespace GoogleCalendarService.Manager
{
    public class EventManager : IEventManager
    {        
        private readonly IBookingService _bookingService;

        public EventManager(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public List<Event> CheckEvent(string calendarId, Event eventToAdd)
        {
            var eventList = _bookingService.GetEvents(calendarId);

            List<Event> eventToAddRecurrentList = GetEventList(eventToAdd);

            var allExistingEvents =
                eventList.SelectMany(GetEventList).ToList();
            var conflictedEvents =
                eventToAddRecurrentList.SelectMany(ev => FindConflictedEvents(allExistingEvents, ev)).ToList();

            return conflictedEvents.DistinctBy(ev => ev.Id).ToList();
        }

        private List<Event> GetEventList(Event eventToAdd)
        {
            if (eventToAdd.Recurrence == null)
                return new EventsListBuilder()
                    .SetSingleEvent(eventToAdd)
                    .SetRealEvent(eventToAdd)
                    .WithInsertedParameters()
                    .Build();

            string pattern =
                @":FREQ=(?<FREQ>\w+);(COUNT=(?<COUNT>\w+);)?(COUNT=(?<COUNT>\w+))?(UNTIL=(?<UNTIL>\d+\w+\d+\w+);)?(UNTIL=(?<UNTIL>\d+\w+\d+\w+))?(BYDAY=(?<BYDAY>[\w,]+))?";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(eventToAdd.Recurrence[0]);

            return new EventsListBuilder()
                .SetFrequencyEnumaretion(match.Groups["FREQ"].Value)
                .SetRealEvent(eventToAdd)
                .SetCount(match.Groups["COUNT"].Value)
                .SetUntil(match.Groups["UNTIL"].Value)
                .SetByDay(match.Groups["BYDAY"].Value)
                .WithInsertedParameters()
                .Build();
        }

        private IEnumerable<Event> FindConflictedEvents(List<Event> eventList, Event eventToAdd)
        {
            return eventList.Where(ev =>
                (
                    (eventToAdd.LeftIntersection(ev)  || 
                     eventToAdd.InnerIntersection(ev) ||
                     eventToAdd.RightIntersection(ev) ||
                     eventToAdd.OuterIntersection(ev)) &&
                    (eventToAdd.Id != ev.Id)
                    ));
        } 
    }
}