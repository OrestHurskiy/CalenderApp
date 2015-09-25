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
    public class UpdateEventController : BaseEventController
    {
        public UpdateEventController(BookingService bookingService)
            : base(bookingService)
        {

        }
        [HttpPut]
        public HttpResponseMessage UpDateEvent(CalendarEvent eventForUpdate, string eventID)
        {
            BookingService.UpdateEvent(ToEventConverter.ToEvent(eventForUpdate), eventForUpdate.CalendarID, eventID);
            return Request.CreateResponse(HttpStatusCode.Created, eventForUpdate);
        }
    }
}