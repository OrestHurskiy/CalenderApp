using System.Collections.Generic;
using BookingRoom.Models.GoogleCalendar;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models.GoogleEvent;
using GoogleCalendarService;
using BookingRoom.Filters;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService.Manager;

namespace BookingRoom.Controllers
{
    [GoogleExceptionFilter]
    public class PostEventController : BaseEventController
    {
        public PostEventController(IBookingService bookingService, IEventManager manager)
            : base(bookingService, manager)
        {

        }
        public HttpResponseMessage PostEvent(CalendarEvent eventPost)
        {
            string timeZone = BookingService.GetCalendarTimeZone(eventPost.CalendarID);
            if (eventPost.ForcedEvent == false)
            {
                Event eventToCheck = ToEventConverter.ToEvent(eventPost, timeZone);
                List<Event> conflictedEvents = EventManager.CheckEvent(eventPost.CalendarID, eventToCheck);

                if (conflictedEvents.Count != 0)
                {
                    var unsuccessfulRequest = new { Successful = false, ConflictedList = conflictedEvents };
                    return Request.CreateResponse(HttpStatusCode.OK, unsuccessfulRequest);
                }
            }
            
            BookingService.PostEvent(ToEventConverter.ToEvent(eventPost, timeZone), eventPost.CalendarID);
            var successfulRequest = new { Successful = true };
            return Request.CreateResponse(HttpStatusCode.Created, successfulRequest);
        }
    }
}