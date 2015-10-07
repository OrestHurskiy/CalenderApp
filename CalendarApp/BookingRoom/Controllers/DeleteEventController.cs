using System.Net;
using System.Net.Http;
using System.Web.Http;
using GoogleCalendarService;
using BookingRoom.Filters;

namespace BookingRoom.Controllers
{
    [GoogleExceptionFilter]
    public class DeleteEventController : BaseEventController
    {
        public DeleteEventController(BookingService bookingService)
            :base(bookingService)
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
