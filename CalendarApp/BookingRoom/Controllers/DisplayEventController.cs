using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using BookingRoom.Filters;
using GoogleCalendarService.Manager;

namespace BookingRoom.Controllers
{
    [GoogleExceptionFilter]
    public class DisplayEventController : BaseEventController
    {
        public DisplayEventController(IBookingService bookingService, IEventManager manager)
            :base(bookingService,manager)
        {

        }

        [HttpGet]
        public HttpResponseMessage DisplayEvents(string calendarId)
        {
            IList<Event> events;
            events = BookingService.GetEvents(calendarId);
            return Request.CreateResponse(HttpStatusCode.OK, events);
        }

    }
}
