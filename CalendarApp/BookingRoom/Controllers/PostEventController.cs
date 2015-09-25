using BookingRoom.Models.GoogleCalendar;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models.GoogleEvent;
using GoogleCalendarService;
using BookingRoom.Filters;

namespace BookingRoom.Controllers
{
    [GoogleExceptionFilter]
    public class PostEventController : BaseEventController
    {
        public PostEventController(BookingService bookingService)
            : base(bookingService)
        {

        }
        [HttpPost]
        public HttpResponseMessage EventPost(CalendarEvent eventPost)
        {
            BookingService.PostEvent(ToEventConverter.ToEvent(eventPost), eventPost.CalendarID);
            return Request.CreateResponse(HttpStatusCode.Created, eventPost);
        }
    }
}
