using System.Net;
using System.Net.Http;
using System.Web.Http;
using GoogleCalendarService;
using BookingRoom.Filters;
using Google.Apis.Calendar.v3;
using GoogleCalendarService.Manager;

namespace BookingRoom.Controllers
{
    [GoogleExceptionFilter]
    public class DeleteEventController : BaseEventController
    {
        public DeleteEventController(IBookingService bookingService, IEventManager manager)
            :base(bookingService, manager)
        {

        }
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(string calendarId, string eventId)
        {
            BookingService.DeleteEvent(calendarId, eventId);
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
