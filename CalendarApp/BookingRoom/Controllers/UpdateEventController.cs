using System.Collections.Generic;
using BookingRoom.Models.GoogleCalendar;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models.GoogleEvent;
using GoogleCalendarService;
using BookingRoom.Filters;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService.Manager;

namespace BookingRoom.Controllers
{
    [GoogleExceptionFilter]
    public class UpdateEventController : BaseEventController
    {
        public UpdateEventController(IBookingService bookingService, IEventManager manager)
            : base(bookingService, manager)
        {

        }
        [HttpPut]
        public HttpResponseMessage UpdateEvent(CalendarEvent eventToUpdate, string eventID)
        {
            string timeZone = BookingService.GetCalendarTimeZone(eventToUpdate.CalendarID);
            if (eventToUpdate.ForcedEvent == false)
            {
                Event eventToCheck = ToEventConverter.ToEvent(eventToUpdate, timeZone);
                eventToCheck.Id = eventID;
                List<Event> conflictedEvents = EventManager.CheckEvent(eventToUpdate.CalendarID, eventToCheck);
                if (conflictedEvents.Count != 0)
                {
                    var unsuccessfulRequest = new { Successful = false, ConflictedList = conflictedEvents };
                    return Request.CreateResponse(HttpStatusCode.OK, unsuccessfulRequest);
                }
            }

            BookingService.UpdateEvent(ToEventConverter.ToEvent(eventToUpdate, timeZone), eventToUpdate.CalendarID, eventID);
            var successfulRequest = new { Successful = true };
            return Request.CreateResponse(HttpStatusCode.Created, successfulRequest);
        }
    }
}